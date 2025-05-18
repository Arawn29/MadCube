using UnityEngine;

public interface IPlayerInteractablePoints
{
    void Interact(GameObject obj);
    int RequiredSensorDetection { get; set; }
}