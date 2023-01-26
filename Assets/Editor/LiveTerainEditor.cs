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


        DrawDefaultInspector();

    }
}
