using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class FallingGround : MonoBehaviour, IPlayerInteractablePoints
{
    [SerializeField] private int requiredSensors;
    public int RequiredSensorDetection { get => requiredSensors; set => requiredSensors = value; }

    public void Interact(GameObject obj)
    {
        Player playerScript = obj.GetComponent<Player>();
        if (playerScript != null)
        {
            bool isUpside = playerScript.GetPlayerOrientation() == "Y" ? true : false;
            if (isUpside)
            {
                InitiateFall();
                MainEvents.Instance.OnPlayerFall?.Invoke();
                
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
}
