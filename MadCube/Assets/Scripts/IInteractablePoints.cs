using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractablePoints
{
    void Interact(GameObject obj);
    int RequiredSensorDetection { get; set; }
}