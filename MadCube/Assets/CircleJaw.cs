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

    private Vector3 pivotPoint;
    private GameObject pivot;
    private void OnEnable()
    {
        MainEvents.Instance.OnPlatformSpawned += AnimateSwing;
    }
    private void OnDisable()
    {
        MainEvents.Instance.OnPlatformSpawned -= AnimateSwing;
    }   
    private void Start()
    {
        pivotPoint = transform.position + (RotationDirection.normalized * rotationRadius);
        pivot = new GameObject("Pivot");
        transform.SetParent(pivot.transform);
        pivot.transform.position = pivotPoint;
        pivot.transform.rotation = Quaternion.AngleAxis(-rotationAngle, rotationAxis);
        AnimateSwing();


    }
    private void Update()
    {
        transform.Rotate(rotationAxis.normalized, 360 * Time.deltaTime, Space.World);
    }
    private void AnimateSwing()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(pivot.transform.DORotate(rotationAxis.normalized * rotationAngle, rotationDuration).SetEase(Ease.InOutSine));
        sequence.AppendInterval(SequenceDelay);
        sequence.Append(pivot.transform.DORotate(-rotationAxis.normalized * rotationAngle, rotationDuration).SetEase(Ease.InOutSine));
        sequence.AppendInterval(SequenceDelay);
        sequence.SetLoops(-1);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            MainEvents.Instance.OnPlayerFall();

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (RotationDirection.normalized * rotationRadius), 0.1f);
    }
}
