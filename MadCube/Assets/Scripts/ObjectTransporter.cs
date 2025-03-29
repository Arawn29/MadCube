using System.Collections;
using UnityEngine;

public class ObjectTransporter : MonoBehaviour, IInteractablePoints
{
    [SerializeField] private GameObject targetObj;
    [SerializeField] private int requiredActivitonSensors;
    [SerializeField] private float tranportTime;
    public int RequiredSensorDetection { get => requiredActivitonSensors; set => requiredActivitonSensors = value; }

    public void Interact(GameObject obj)
    {
        StartCoroutine(Transport(obj));
    }

    IEnumerator Transport(GameObject obj)
    {
        Debug.Log("Starting Transport Procces");
        float elapsedTime = 0f;
        Vector3 startPosition = obj.transform.position;
        Vector3 finalPosition = CalculateFinalPosition(obj);
        while (elapsedTime <= tranportTime)
        {
            obj.transform.position = Vector3.Lerp(startPosition, finalPosition, elapsedTime / tranportTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = finalPosition;
    }
    Vector3 CalculateFinalPosition(GameObject @object)
    {float offsetMultiplierY = (targetObj.GetComponent<BoxCollider>().size.y / 2) + (@object.GetComponent<BoxCollider>().size.y / 2);

        return targetObj.transform.position + Vector3.up * offsetMultiplierY;

    }
}
