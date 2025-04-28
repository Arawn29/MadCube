using UnityEngine;
using UnityEditor;
/*MenuTool
public class PlatformEditorTools
{
    [MenuItem("Tools/MyTools/Seçili Objenin Materyalini Uygula")]
    public static void ApplyMaterialToSelectedPlatform()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("Hiçbir obje seçilmedi.");
            return;
        }

        Platform platform = Selection.activeGameObject.GetComponent<Platform>();
        if (platform == null)
        {
            Debug.LogWarning("Seçili obje Platform scriptine sahip deðil.");
            return;
        }

        platform.ApplyMaterialToChildren();

        // Deðiþikliði editöre bildir
        EditorUtility.SetDirty(platform.gameObject);
        Debug.Log("Materyal uygulandý (Tools menüsünden).");
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