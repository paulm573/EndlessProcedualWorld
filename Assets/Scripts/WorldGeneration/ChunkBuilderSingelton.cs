using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;


public class ChunkBuilderSingelton : MonoBehaviour
{

    private const int maxCachedChunks = 200;

    public static ChunkBuilderSingelton Instance;
    Queue<MapThreadInfo<ChunkInfo>> mapDataThreadQueue = new Queue<MapThreadInfo<ChunkInfo>>();
    CachingDic<Vector2, float[,]> heightBuffer= new CachingDic<Vector2,float[,]>(maxCachedChunks);

    private void Awake()
    {
        Instance = this;

        //int a, b;
        //ThreadPool.SetMaxThreads(3, 3);
        //ThreadPool.GetAvailableThreads(out a, out b);
        //Debug.Log($"a{a},b{b}");
    }
    private void Update()
    {   
        if (mapDataThreadQueue.Count <= 0) { return; } // guard

        for (int i = 0; i < mapDataThreadQueue.Count; i++)
        {
            MapThreadInfo<ChunkInfo> threadInfo_ = mapDataThreadQueue.Dequeue();
            threadInfo_.callback(threadInfo_.param);
        }
    }

    public void RequestChunkData(Action<ChunkInfo> callback, Vector2 position, int detailLevel)
    {
        //ThreadStart threadStart = delegate { ChunkBuilderDataThread(callback, position, detailLevel); };
        //new Thread(threadStart).Start();
        ThreadPool.QueueUserWorkItem(delegate { ChunkBuilderDataThread(callback, position, detailLevel); });
    }

    private void ChunkBuilderDataThread(Action<ChunkInfo> callback, Vector2 pos, int detailLevel)
    {
        ChunkInfo mapData = GenerateChunk(detailLevel,pos);
        lock (mapDataThreadQueue)
        { mapDataThreadQueue.Enqueue(new MapThreadInfo<ChunkInfo>(callback, mapData)); }
    }

    private ChunkInfo GenerateChunk(int detailLevel,Vector2 position)
    {   
        (Vector3[], int[]) mesh = CreateChunkMesh(detailLevel,position);
        int f = 0;
        BiomeStruct biome = TerainSettings.Instance.useBiomes[f];
        Color[] meshColors = MeshPainter.GenerateMeshColors(TerainSettings.Instance.seed,mesh.Item1,detailLevel, biome.heightlevels, biome.blendStrengths, biome.colors, biome.colorVariation);
        return new ChunkInfo(mesh.Item1,mesh.Item2,meshColors);
    }


    private (Vector3[], int[]) CreateChunkMesh(int detailLevel, Vector2 position)
    {
        string heightMapId = $"{position.y}_{position.x}";

        lock (heightBuffer)
        {
            if (heightBuffer.ContainsKey(position))
            {
                return MeshGenerator.GenerateMeshFromHeightMap(heightBuffer.Get(position), detailLevel);
            }
        }




        int size = TerainSettings.Instance.chunkSize_ * 12 + 1;

        // Continetalness
        float[,] continental;
        continental = NoiseGenerator.Generate2DNoiseMap(TerainSettings.Instance.seed, size, TerainSettings.Instance.c_noiseScale, TerainSettings.Instance.c_octaves, TerainSettings.Instance.c_persistance, TerainSettings.Instance.c_lacunarity,TerainSettings.Instance.c_curve.keys, position);

        // Peaks
        float[,] peaksAndValleys;
        peaksAndValleys = NoiseGenerator.Generate2DNoiseMap(TerainSettings.Instance.seed +10, size, TerainSettings.Instance.p_noiseScale, TerainSettings.Instance.p_octaves, TerainSettings.Instance.p_persistance, TerainSettings.Instance.p_lacunarity, TerainSettings.Instance.p_curve.keys, position);

        // Erosion
        float[,] erosionMap;
        erosionMap = NoiseGenerator.Generate2DNoiseMap(TerainSettings.Instance.seed + 20, size, TerainSettings.Instance.e_noiseScale, TerainSettings.Instance.e_octaves, TerainSettings.Instance.e_persistance, TerainSettings.Instance.e_lacunarity, TerainSettings.Instance.e_curve.keys, position);


        float[,] heightMap = new float[size, size];
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {   
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if (!TerainSettings.Instance.c_on) { continental[x, z] = 1; }
                if (!TerainSettings.Instance.e_on) { erosionMap[x, z] = 1f; }
                if (!TerainSettings.Instance.p_on) { peaksAndValleys[x, z] = 1; }
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //heightMap[x, z] = ((peaksAndValleys[x, z] * continental[x, z]) * TerainSettings.Instance.c_amplitude) + erosionMap[x,z] * TerainSettings.Instance.e_amplitude;
                heightMap[x, z] = (TerainSettings.Instance.c_amplitude * continental[x,z] + peaksAndValleys[x,z] * TerainSettings.Instance.p_amplitude) * erosionMap[x, z] ;
            }
        }

        // cache heightMap
        lock (heightBuffer) { heightBuffer.Cache(position, heightMap); }



        return MeshGenerator.GenerateMeshFromHeightMap(heightMap, detailLevel);
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T param;

        public MapThreadInfo(Action<T> callback, T param)
        {
            this.callback = callback;
            this.param = param;
        }
    }

}


