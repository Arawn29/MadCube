using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IPlayerInteractablePoints
{
    [SerializeField] private int requiredSensorDetection;
    [SerializeField] private GameObject[] associatedObject;

    bool isPressed = false;
    Material material;
    public int RequiredSensorDetection { get => requiredSensorDetection; set => requiredSensorDetection = value; }

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }
    public void Interact(GameObject obj)
    {
        if (isPressed) return;
        Debug.Log("isbuttonpressed true ");
        isPressed = true;
        foreach (var item in associatedObject)
        {
            if (item.TryGetComponent(out IButtonListener buttonListener))
            {
                buttonListener.ButtonPressed();
            }
        }
        
        ChangeMaterialColor();
    }

    private void ChangeMaterialColor()
    {
        Debug.Log("MateriaL coLor have to change");
        material.color = Color.green;
    }
}
