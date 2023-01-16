using UnityEditor;

[CustomEditor(typeof(NoiseMapGenerator))]
public class NoiseMapGenEditor : Editor
{

    public override void OnInspectorGUI()
    {
        NoiseMapGenerator noiseMapGenerator = (NoiseMapGenerator)target;

        if (DrawDefaultInspector())
        {
            noiseMapGenerator.GenerateMap();
        };


    }
}