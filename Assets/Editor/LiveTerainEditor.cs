using UnityEditor;

[CustomEditor(typeof(TerainSettings))]
public class LiveTerainEditor : Editor
{

    public override void OnInspectorGUI()
    {
        TerainSettings generator = (TerainSettings)target;

        if (DrawDefaultInspector())
        {
            generator.Generate();
        };


    }
}
