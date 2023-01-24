using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(ChunkBuilderSingelton))]
[System.Serializable]
public class TerainSettings : MonoBehaviour
{
    [SerializeField]
    public static TerainSettings Instance;

    public string loadSave;

    [Header("General")]
    [SerializeField]                 public int seed;
    [SerializeField]                 public Transform worldRoot;
    [SerializeField][Range(1, 20)]   public int chunkSize_;
    

    [Header("Continetalness")]
    [SerializeField]                 public bool c_on;
    [SerializeField][Range(1, 1000)] public int c_amplitude;
    [SerializeField][Range(1, 2000)]  public float c_noiseScale;
    [SerializeField][Range(1, 10)]   public int   c_octaves;
    [SerializeField][Range(0, 1)]    public float c_persistance;
    [SerializeField][Range(1, 5)]    public float c_lacunarity;
    [SerializeField]                 public AnimationCurve c_curve;

    [Header("Erosion")]
    [SerializeField] public bool e_on;
    [SerializeField][Range(1, 500)]  public int e_amplitude;
    [SerializeField][Range(1, 500)]  public float e_noiseScale;
    [SerializeField][Range(1, 10)]   public int   e_octaves;
    [SerializeField][Range(0, 1)]    public float e_persistance;
    [SerializeField][Range(1, 5)]    public float e_lacunarity;
    [SerializeField]                 public AnimationCurve e_curve;

    [Header("Peaks")]
    [SerializeField] public bool p_on;
    [SerializeField][Range(1, 500)]  public float p_noiseScale;
    [SerializeField][Range(1, 10)]   public int   p_octaves;
    [SerializeField][Range(0, 1)]    public float p_persistance;
    [SerializeField][Range(1, 5)]    public float p_lacunarity;
    [SerializeField]                 public AnimationCurve p_curve;

    [Header("BiomeSettings")]
    [SerializeField]                 public int estimatedMinHeight;
    [SerializeField]                 public int estimatedMaxHeight;
    [SerializeField]                 public Biome[] biomes;

    [Header("LOD-System")]
    [SerializeField]                 public int lod_6;
    [SerializeField]                 public int lod_5;
    [SerializeField]                 public int lod_4;
    [SerializeField]                 public int lod_3;
    [SerializeField]                 public int lod_2;
    [SerializeField]                 public int lod_1;
    [SerializeField]                 public int lod_disable;
    [SerializeField]                 public int lod_delete;

    [Header("In-Editor-Preview-Only")]
    [SerializeField][Range(1, 6)] int detailLevel;
    [SerializeField][Range(1, 100)] int chunkCount;
    [SerializeField] int yScrol;
 
    
    

    private void Awake()
    {
        Instance = this;
        foreach (Biome biome in biomes)
        {
            biome.UpdateTextures();
        }
    }























    // For Generation inside SceneView Only
    private Queue<Chunk> chunks = new Queue<Chunk> {};
    public void Generate(){

        foreach (Biome biome in biomes)
        {
            biome.UpdateTextures();
        }

        while (chunks.Count > 0)
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
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public void Save() { 

        BinaryFormatter formatter= new BinaryFormatter();
        string path = "Assets/WorldPresets/" + loadSave + ".world";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, Instance);
        stream.Close();


    }

    public void Load() {
        string path = "Assets/WorldPresets/" + loadSave;
        if(File.Exists(path))
        {
            BinaryFormatter formatter= new BinaryFormatter();
            FileStream fileStream = new FileStream(path,FileMode.Open);

            Instance = formatter.Deserialize(fileStream) as TerainSettings;
            fileStream.Close();
        }
    }
}
