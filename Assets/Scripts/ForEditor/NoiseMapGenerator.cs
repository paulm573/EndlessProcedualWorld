using UnityEngine;

public class NoiseMapGenerator : MonoBehaviour
{
    public int seed;
    [Range(1, 1000)] [SerializeField] int mapSize;
    [Range(1, 1000)][SerializeField] float scaleL1;
    [Range(1, 10)]  [SerializeField] int octaves;
    [Range(0, 1)]   [SerializeField] float persistance;
    [Range(1, 10)]  [SerializeField] float lacunarity;
    [SerializeField] AnimationCurve curve;
    [SerializeField] int xOff;
    [SerializeField] int zOff;

    public Renderer textureRenderer;

    public void GenerateMap()
    {
        float[,] noiseMap = NoiseGenerator.Generate2DNoiseMap(seed, mapSize, scaleL1, octaves, persistance, lacunarity, curve.keys,new Vector2(xOff, zOff)) ;
        DrawNoiseMap(noiseMap);
    }

    public void DrawNoiseMap(float[,] noiseMap)
    {
        int size = noiseMap.GetLength(0);

        Texture2D texture = new Texture2D(size, size);
        Color[] colourMap = new Color[size * size];

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                colourMap[z * size + x] = Color.Lerp(new Color(0, 1, 0, 0f), new Color(1, 0, 0, 1f), noiseMap[z, x]);
                //colourMap[z*size+x] = Color.Lerp(Color.white,Color.black,noiseMap[z,x]);
            }
        }
        texture.SetPixels(colourMap);
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(size, 1, size);
    }

}