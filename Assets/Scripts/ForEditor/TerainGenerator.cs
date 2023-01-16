using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerainGenerator : MonoBehaviour
{
    [SerializeField]                 int seed;
    [SerializeField][Range(1, 6)]    int detailLevel;
    [SerializeField][Range(1, 20)]   int chunkSize_;
    [SerializeField][Range(1, 100)]  int chunkCount;
    [SerializeField][Range(1, 1000)] int amplitude;
    [SerializeField][Range(1, 250)]  float scaleL1;
    [SerializeField][Range(1, 10)]   int octaves;
    [SerializeField][Range(0, 1)]    float persistance;
    [SerializeField][Range(1, 5)]    float lacunarity;

    
    public void Generate(){

        // Convert Values 
        int chunkSize = 12 * chunkSize_;
        int levelSize = (int)Mathf.Sqrt(chunkCount);

        
        for (int z= 0;z < levelSize; z++)
        {
            for (int x = 0; x < levelSize; x++)
            {
                // make the mesh
                float[,] heightMap = NoiseGenerator.Generate2DNoiseMap(seed, chunkSize, scaleL1, octaves, persistance, lacunarity, new Vector2(x, z));
                GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateMeshFromHeightMap(heightMap,amplitude,detailLevel);
            }
        }
       
    }
}