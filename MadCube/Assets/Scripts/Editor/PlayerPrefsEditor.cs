using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerPrefsEditor 
{
    [MenuItem("Tools/ClearRecord")]
    public static void ClearRecord()
    {
        PlayerPrefs.DeleteKey("Time");
        Debug.Log("Record cleared.");
    }
}
