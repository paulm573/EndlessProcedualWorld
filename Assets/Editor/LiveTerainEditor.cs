using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerainSettings))]
public class LiveTerainEditor : Editor
{

    public override void OnInspectorGUI()
    {
        TerainSettings generator = (TerainSettings)target;


       
        
            if (GUILayout.Button("Generate")) 
            { 
                generator.Generate();
            }
        if (GUILayout.Button("Load"))
        {
            generator.Load();
        }
        if (GUILayout.Button("Save"))
        {
            generator.Save();
        }

        DrawDefaultInspector();

    }
}
