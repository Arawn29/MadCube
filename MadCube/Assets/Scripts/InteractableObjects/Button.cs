using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Button : MonoBehaviour, IPlayerInteractablePoints
{
    const string BUTTON_COLOR_STR = "_Base_Color";
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
        
        ChangeMaterialStats();
    }

    private void ChangeMaterialStats()
    {
        Debug.Log("MateriaL coLor have to change");
        material.SetColor(BUTTON_COLOR_STR, Color.green);
        transform.DOMoveY(transform.position.y - 0.1f, 0.5f).SetEase(Ease.OutBounce);
    }
}
