using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChunkBuilderSingelton))]
public class TerainSettings : MonoBehaviour
{
    public static TerainSettings Instance;

    [SerializeField]                 public int seed;
    [SerializeField][Range(1, 20)]   public int chunkSize_;
    [SerializeField][Range(1, 1000)] public int amplitude;
    [SerializeField][Range(1, 250)]  public float noiseScale;
    [SerializeField][Range(1, 10)]   public int octaves;
    [SerializeField][Range(0, 1)]    public float persistance;
    [SerializeField][Range(1, 5)]    public float lacunarity;
    [SerializeField][Range(0, 100)]  public int maxViewDistance;

    [SerializeField][Range(1, 6)] int detailLevel;
    [SerializeField][Range(1, 100)] int chunkCount;
    [SerializeField] int yScrol;
    [SerializeField] public Transform world;

    private void Awake()
    {
        Instance = this;
    }

    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public Material placeholderMat;
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    // For Generation inside SceneView Only
    private Queue<Chunk> chunks = new Queue<Chunk> {};
    public void Generate(){

        while(chunks.Count > 0)
        {
            Chunk chunk = chunks.Dequeue();
            chunk.SelfDestroy();
        }

        // Convert Values 
        int chunkSize = 12 * chunkSize_;
        int levelSize = (int)Mathf.Sqrt(chunkCount);

        
        for (int z= 0 + yScrol;z < levelSize+yScrol; z++)
        {
            for (int x = 0; x < levelSize; x++)
            {
                chunks.Enqueue(new Chunk(new Vector2(x, z),detailLevel));
            }
        }
       
    }
}