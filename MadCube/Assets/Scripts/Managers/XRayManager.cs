using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class XRayManager : MonoBehaviour
{
    public static readonly float XraySmoothTime = 0.7f;
    public static readonly float XraySize = 4f;
    public static int GroundSizeIDXray = Shader.PropertyToID("_SizeXray");
    public static int PlayerPositionXRay = Shader.PropertyToID("_PlayerPositionXray");

    private Coroutine XrayCoroutine;
    private List<Material> EffectedMaterials = new List<Material>();

    public void DetermineXRayFeasibility(GameObject player, LayerMask groundLayerMask)
    {
        if (Camera.main == null || player == null || XrayCoroutine != null) return;

        Vector3 direction = (player.transform.position - Camera.main.transform.position);
        float distance = Vector3.Distance(Camera.main.transform.position, player.transform.position);
        RaycastHit hit;

        if (Physics.BoxCast(Camera.main.transform.position, new Vector3(1f, 0.1f, 1f), direction, out hit, Quaternion.identity, distance * 0.75f, groundLayerMask))
        {
            Debug.Log("Bir cisim var");
            Renderer renderer = hit.collider.transform.GetChild(0).GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = renderer.sharedMaterial;
                if ( mat != null && !EffectedMaterials.Contains(mat))
                {
                    EffectedMaterials.Add(mat);
                    XrayCoroutine = StartCoroutine(SetXrayMaterialProperties(mat, player));
                }
            }
        }
        else
        {
            Debug.Log("Bir cisim Yok");
            ResetMaterials();
        }
    }
    private IEnumerator SetXrayMaterialProperties(Material xrayMaterial, GameObject player)
    {
        float elapsedTime = 0f;

        while (elapsedTime <= XraySmoothTime)
        {
            elapsedTime += Time.deltaTime;
            float step = Mathf.Lerp(0f, XraySize, elapsedTime / XraySmoothTime);
            xrayMaterial.SetFloat(GroundSizeIDXray, step);
            yield return null;
        }

        elapsedTime = 0f;
        var targetview = Camera.main.WorldToViewportPoint(player.transform.position);
        var currentview = xrayMaterial.GetVector(PlayerPositionXRay);
        while (elapsedTime <= XraySmoothTime)
        {
            elapsedTime += Time.deltaTime;
            currentview = Vector4.Lerp(currentview, targetview, elapsedTime / XraySmoothTime);
            xrayMaterial.SetVector(PlayerPositionXRay, currentview);
            yield return null;
        }
        XrayCoroutine = null;
    }

    private void ResetMaterials()
    {
        List<Material> materialsToRemove = new List<Material>();

        foreach (var item in EffectedMaterials)
        {
            if (item == null) continue;
            item.SetFloat(GroundSizeIDXray, 0f);
            materialsToRemove.Add(item);
        }

        foreach (var material in materialsToRemove)
        {
            EffectedMaterials.Remove(material);
        }
    }
    public void ResetMaterialProperties(Material material)
    {
        material.SetFloat(GroundSizeIDXray, 0f);
    }

}
