using UnityEngine;
using UnityEditor;
/*MenuTool
public class PlatformEditorTools
{
    [MenuItem("Tools/MyTools/Se�ili Objenin Materyalini Uygula")]
    public static void ApplyMaterialToSelectedPlatform()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("Hi�bir obje se�ilmedi.");
            return;
        }

        Platform platform = Selection.activeGameObject.GetComponent<Platform>();
        if (platform == null)
        {
            Debug.LogWarning("Se�ili obje Platform scriptine sahip de�il.");
            return;
        }

        platform.ApplyMaterialToChildren();

        // De�i�ikli�i edit�re bildir
        EditorUtility.SetDirty(platform.gameObject);
        Debug.Log("Materyal uyguland� (Tools men�s�nden).");
    }
}

*/



[CustomEditor(typeof(Platform))]
public class PlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        Platform platform = (Platform)target;
        platform.PlatformIndex = EditorGUILayout.IntField("Platform Index", platform.PlatformIndex);
        platform.material = (Material)EditorGUILayout.ObjectField("Material", platform.material, typeof(Material), true);
        if (GUILayout.Button("Apply Material"))
        {
            platform.ApplyMaterialToChildren();
        }
    }
}