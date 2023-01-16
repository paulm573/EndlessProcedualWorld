using UnityEditor;

[CustomEditor(typeof(TerainGenerator))]
public class LiveTerainEditor : Editor
{

    public override void OnInspectorGUI()
    {
        TerainGenerator generator = (TerainGenerator)target;

        if (DrawDefaultInspector())
        {
            generator.Generate();
        };


    }
}
