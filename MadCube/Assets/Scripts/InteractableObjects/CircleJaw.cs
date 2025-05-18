using DG.Tweening;
using UnityEngine;

public class CircleJaw : MonoBehaviour
{
    [SerializeField] private Vector3 RotationDirection = Vector3.right;
    [SerializeField] private float rotationRadius = 1f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationAngle = 45f;
    [SerializeField] private float rotationDuration = 1f;
    [SerializeField] private float SequenceDelay = 1f;

    int platformIndex;
    private Vector3 pivotPoint;
    private GameObject pivot;

    private void Awake()
    {
        Platform platform = GetComponentInParent<Platform>();
        if (platform != null)
        {
            platformIndex = platform.PlatformIndex;
        }
    }
    private void OnEnable()
    {

        MainEvents.Instance.OnPlatformSpawned += AnimateSwing;
    }
    private void OnDisable()
    {
        MainEvents.Instance.OnPlatformSpawned -= AnimateSwing;
    }
    private void Update()
    {
        transform.Rotate(rotationAxis.normalized, 360 * Time.deltaTime, Space.World);
    }
    private void AnimateSwing(int spawnedPlatformIndex)
    {
        if (spawnedPlatformIndex != platformIndex) return;
        Debug.Log("Çalýþtý");
        pivotPoint = transform.position + (RotationDirection.normalized * rotationRadius);
        pivot = new GameObject("CircleJawPivot");
        pivot.transform.parent = transform.parent;
        pivot.transform.position = pivotPoint;
        transform.SetParent(pivot.transform);
        pivot.transform.rotation = Quaternion.AngleAxis(-rotationAngle, rotationAxis);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(pivot.transform.DORotate(rotationAxis.normalized * rotationAngle, rotationDuration).SetEase(Ease.InOutSine));
        sequence.AppendInterval(SequenceDelay);
        sequence.Append(pivot.transform.DORotate(-rotationAxis.normalized * rotationAngle, rotationDuration).SetEase(Ease.InOutSine));
        sequence.AppendInterval(SequenceDelay);
        sequence.SetLoops(-1);
    }

    bool isHitted = false;
    private void OnCollisionEnter(Collision other)
    {
        if (isHitted) return;
        if (other.collider.CompareTag("Player"))
        {
            if (other.collider.TryGetComponent(out Player playerScript))
            {
                ChangeisHitted();
                Debug.Log("CircleJaw hitted");
                playerScript.StopRoll();
                MainEvents.Instance.OnPlayerFalled?.Invoke();

            }
            while (true)
            {
                if (other.collider.TryGetComponent(out Rigidbody rb))
                {
                    rb.AddExplosionForce(200f, transform.position, 20f);
                    break;
                }
                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Invoke(nameof(ChangeisHitted), 0.5f);
        }


    }
    private void ChangeisHitted()
    {
        isHitted = !isHitted;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (RotationDirection.normalized * rotationRadius), 0.1f);
    }
}
