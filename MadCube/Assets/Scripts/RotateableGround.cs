using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class RotateableGround : MonoBehaviour,IButtonListener
{
    Transform pivotTransform;
    bool isRotated = false;
    Material mat;
    public void ButtonPressed()
    {
        if (isRotated) return;
        Rotate();
        isRotated = true;
    }
    private void Awake()
    {
        pivotTransform = transform.Find("Pivot");
        pivotTransform.transform.parent = null;
        transform.parent = pivotTransform.transform;
        Renderer renderer = transform.Find("Cube").transform.GetComponent<Renderer>();
        mat = renderer.material;
        if (mat != null)
        {
            mat.SetColor("_EmissionColor", Color.red);
        }
    }

    private void Rotate()
    {
        if (pivotTransform == null) return;
        pivotTransform.transform.DORotate(Vector3.zero, 1f).SetEase(Ease.InOutSine);
        if (mat == null) return;
        mat.SetColor("_EmissionColor", Color.green);

    }
}
