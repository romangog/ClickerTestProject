using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(EcsStartup))]
public class EcsStartupInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Delete Save"))
        {
            string savePath = Application.persistentDataPath + "save";
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
        }
    }
}
