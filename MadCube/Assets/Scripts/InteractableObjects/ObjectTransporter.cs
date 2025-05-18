using System;
using DG.Tweening;
using UnityEngine;

public class ObjectTransporter : MonoBehaviour, IPlayerInteractablePoints, IButtonListener
{
    [SerializeField] private GameObject transportTarget;
    [SerializeField] private int requiredActivitonSensors;
    [SerializeField] private float transportTime;
    [SerializeField] private int requiredActivateButton;
    int currentButton = 0;   // Burada property olmasýna gerek yok interfaceden gelen deðeri sadece bu script içerisinde okuyacaðýmýz için nemaproblema
    public int RequiredSensorDetection { get => requiredActivitonSensors; set => requiredActivitonSensors = value; }
    bool isTransported;
    public void Interact(GameObject obj)
    {
        if (currentButton >= requiredActivateButton)
        {
            Transport(obj);
        }
    }

    public void ButtonPressed()
    {
        currentButton++;
    }

    public void Transport(GameObject obj)
    {
        if(isTransported) return;
        GameManager.Instance.ChangeGameState(GameState.Transporting);
        Debug.Log("Starting Transport Procces");
        obj.transform.parent = transform;
        float ySize = transform.GetComponent<BoxCollider>().size.y /2;
        float yObjSize = obj.transform.GetComponent<BoxCollider>().size.y / 2;
        obj.transform.localPosition = Vector3.up* (ySize + yObjSize);
        Vector3 targetPos = transportTarget.transform.position;
        transform.DOMove(targetPos, transportTime).OnComplete(() =>
            {
                isTransported = true;
                GameManager.Instance.DetermineXRayFeasibility();
                GameManager.Instance.ChangeGameState(GameState.Playable);
                MainEvents.Instance.OnPlayerTeleported?.Invoke(obj.transform.position);
                obj.transform.parent = null;
                
            });
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transportTarget.transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transportTarget.transform.position);

    }
}
