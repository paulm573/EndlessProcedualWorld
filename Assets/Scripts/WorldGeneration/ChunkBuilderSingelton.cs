using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

public class ChunkBuilderSingelton : MonoBehaviour
{
    public static ChunkBuilderSingelton Instance;
    Queue<MapThreadInfo<ChunkInfo>> mapDataThreadQueue = new Queue<MapThreadInfo<ChunkInfo>>();

    private void Awake()
    {
        Instance = this;
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
        ThreadStart threadStart = delegate { ChunkBuilderDataThread(callback, position, detailLevel); };
        new Thread(threadStart).Start();
    }

    private void ChunkBuilderDataThread(Action<ChunkInfo> callback, Vector2 pos, int detailLevel)
    {
        ChunkInfo mapData = GenerateChunk(detailLevel,pos);
        lock (mapDataThreadQueue)
        { mapDataThreadQueue.Enqueue(new MapThreadInfo<ChunkInfo>(callback, mapData)); }
    }

    private ChunkInfo GenerateChunk(int detailLevel,Vector2 position)
    {
        float[,] heightMap = NoiseGenerator.Generate2DNoiseMap(TerainSettings.Instance.seed, TerainSettings.Instance.chunkSize_*12+1, TerainSettings.Instance.c_noiseScale, TerainSettings.Instance.c_octaves, TerainSettings.Instance.c_persistance, TerainSettings.Instance.c_lacunarity, position);
        (Vector3[], int[]) mesh = MeshGenerator.GenerateMeshFromHeightMap(heightMap,TerainSettings.Instance.amplitude,detailLevel);
        return new ChunkInfo(mesh.Item1,mesh.Item2);
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


