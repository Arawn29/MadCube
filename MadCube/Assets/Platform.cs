using UnityEngine;

public class Platform : MonoBehaviour
{
    public Material material;

    public int PlatformIndex;
    private void Start()
    {
        GameManager.Instance.xRayManager.ResetMaterialProperties(material);
        RegisterPlatform();
    }

    private void ReplaceAllChildMaterials(Transform parent, Material newMaterial)
    {
        foreach (Transform child in parent)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = newMaterial;
            }
            ReplaceAllChildMaterials(child, newMaterial);
        }
    }

    public void ApplyMaterialToChildren()
    {
        ReplaceAllChildMaterials(transform, material);
    }
    public void DestoryPlatform()
    {
        PlatformManager.Instance.UnregisterPlatformToDýctýonary(PlatformIndex);
        Destroy(gameObject);
    }
    public void RegisterPlatform()
    {
        PlatformManager.Instance.RegisterPlatformToDýctýonary(PlatformIndex, gameObject);
    }
}
