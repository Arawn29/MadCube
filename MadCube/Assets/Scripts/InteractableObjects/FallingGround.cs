using System.Threading.Tasks;
using UnityEngine;

public class FallingGround : MonoBehaviour, IPlayerInteractablePoints
{
    [SerializeField] private int requiredSensors =2;
    public int RequiredSensorDetection { get => requiredSensors; set => requiredSensors = value; }
    Vector3 firstPosition;
    private void Awake()
    {
        firstPosition = transform.position;
    }
    private void OnEnable()
    {
        
        MainEvents.Instance.OnGameRestarting += StopFall;
    }
    private void OnDisable()
    {
        MainEvents.Instance.OnGameRestarting -= StopFall;
    }
    public void Interact(GameObject obj)
    {
        Player playerScript = obj.GetComponent<Player>();
        if (playerScript != null)
        {
            bool isUpside = playerScript.GetPlayerOrientation() == "Y" ? true : false;
            if (isUpside)
            {
                InitiateFall();
                MainEvents.Instance.OnPlayerFalled?.Invoke();

            }
        }
    }
    void InitiateFall()
    {
        if (!transform.TryGetComponent(out Rigidbody rb))
        {
            rb = gameObject.AddComponent<Rigidbody>();

            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
    void StopFall()
    {
        if (transform.TryGetComponent(out Rigidbody rb))
        {
            Destroy(rb);
            transform.position = firstPosition;
            transform.rotation = Quaternion.identity;
            gameObject.SetActive(false);
        }
    }
}
