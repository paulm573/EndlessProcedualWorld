using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public Transform viewer;

    private int chunkSize;
    private Vector2 viewerPosition;
    private int currentChunk_Z, currentChunk_X, old_Z, old_X;
    private int lod_delete, lod_disable, lod_1, lod_2, lod_3, lod_4, lod_5, lod_6;
    private static Dictionary<Vector2, Chunk> chunkDictionary = new Dictionary<Vector2, Chunk>();
    
    private void Start()
    {
        chunkSize = TerainSettings.Instance.chunkSize_*12;
        old_X = int.MaxValue;
        old_Z = int.MaxValue;

        int lodStepsize = TerainSettings.Instance.maxViewDistance / 6;
        lod_delete  = TerainSettings.Instance.maxViewDistance + lodStepsize*2; 
        lod_disable = lod_delete  - lodStepsize;
        lod_1       = lod_disable - lodStepsize;
        lod_2       = lod_1 - lodStepsize;
        lod_3       = lod_2 - lodStepsize;
        lod_4       = lod_3 - lodStepsize;
        lod_5       = lod_4 - lodStepsize;
        lod_6       = lod_5 - lodStepsize;

        //Debug.Log(lod_delete);
        //Debug.Log(lod_disable);
        //Debug.Log(lod_1);
        //Debug.Log(lod_2);
        //Debug.Log(lod_3);
        //Debug.Log(lod_4);
        //Debug.Log(lod_5);
        //Debug.Log(lod_6);
                

    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x / chunkSize, viewer.position.z / chunkSize);
        currentChunk_Z = Mathf.RoundToInt(viewerPosition.y );
        currentChunk_X = Mathf.RoundToInt(viewerPosition.x );

        // Guard save unnecessary Updates
        if(currentChunk_Z == old_Z && currentChunk_X== old_X) { Debug.Log("hi"); return; }

        old_Z= currentChunk_Z;
        old_X= currentChunk_X;

        UpdateVisibleChunks();
    }

    private void UpdateVisibleChunks()
    {
        for (int zOff = -TerainSettings.Instance.maxViewDistance; zOff <= TerainSettings.Instance.maxViewDistance; zOff++)
        {
            for (int xOff = -TerainSettings.Instance.maxViewDistance; xOff <= TerainSettings.Instance.maxViewDistance; xOff++)
            {
                // 0,0 w Grid Format 0,1 ....
                Vector2 currentChunkPos = new Vector2(currentChunk_X + xOff, currentChunk_Z + zOff);
                int detailLevel = CalculateDetailLevel(currentChunkPos, new Vector2(currentChunk_X,currentChunk_Z));

                if (chunkDictionary.ContainsKey(currentChunkPos))
                {
                    if (chunkDictionary[currentChunkPos].IsAlive() == false)
                    {
                        chunkDictionary[currentChunkPos] = new Chunk(currentChunkPos, detailLevel);
                    }
                    else{ 
                        chunkDictionary[currentChunkPos].UpdateChunk(detailLevel);
                    }
                }
                else
                {
                    chunkDictionary.Add(currentChunkPos, new Chunk(currentChunkPos, detailLevel));
                }
            }
        }
    }

    private int CalculateDetailLevel(Vector2 currentChunkPos, Vector2 viewerCoord)
    {
        int detail = (int) (Mathf.Abs(currentChunkPos.x-viewerCoord.x) + Mathf.Abs(viewerCoord.y-currentChunkPos.y));
       
        if      (detail >  lod_delete) { return -2; } // delete
        else if (detail <= lod_6) { return 6; }
        else if (detail <= lod_5) { return 5; }
        else if (detail <= lod_4) { return 4; }
        else if (detail <= lod_3) { return 3; }
        else if (detail <= lod_2) { return 2; }
        else if (detail <= lod_1) { return 1; }
        else if (detail <= lod_disable) { return -1; } // disable

        return -2; //cant be reached
    }

}
