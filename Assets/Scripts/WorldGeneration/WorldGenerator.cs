using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public Transform viewer;

    private int chunkSize;
    private Vector2 viewerPosition;
    private int currentChunk_Z, currentChunk_X, old_Z, old_X;
    private int chunkCheckingDistance;
    private static Dictionary<Vector2, Chunk> chunkDictionary = new Dictionary<Vector2, Chunk>();
    
    private void Start()
    {
        chunkSize = TerainSettings.Instance.chunkSize_*12;
        old_X = int.MaxValue;
        old_Z = int.MaxValue;

        chunkCheckingDistance = TerainSettings.Instance.lod_delete + 1;
    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x / chunkSize, viewer.position.z / chunkSize);
        currentChunk_Z = Mathf.RoundToInt(viewerPosition.y );
        currentChunk_X = Mathf.RoundToInt(viewerPosition.x );

        // Guard save unnecessary Updates
        if(currentChunk_Z == old_Z && currentChunk_X== old_X) { return; }

        old_Z= currentChunk_Z;
        old_X= currentChunk_X;

        UpdateVisibleChunks();
    }

    private void UpdateVisibleChunks()
    {
        for (int zOff = -chunkCheckingDistance; zOff <= chunkCheckingDistance; zOff++)
        {
            for (int xOff = -chunkCheckingDistance; xOff <= chunkCheckingDistance; xOff++)
            {
                // 0,0 w Grid Format 0,1 ....
                Vector2 currentChunkPos = new Vector2(currentChunk_X + xOff, currentChunk_Z + zOff);
                int detailLevel = CalculateDetailLevel(currentChunkPos, new Vector2(currentChunk_X,currentChunk_Z));

                // New Entry
                if (!chunkDictionary.ContainsKey(currentChunkPos))
                {
                    if (detailLevel > -2)
                    {
                        chunkDictionary.Add(currentChunkPos, new Chunk(currentChunkPos, detailLevel));    // Create
                    }
                    continue;
                }

                // Key already contained
                if (detailLevel <= -2)
                {
                    chunkDictionary[currentChunkPos].SelfDestroy();   // Delete
                    chunkDictionary.Remove(currentChunkPos);
                }
                else if (detailLevel <= -1)
                {
                    chunkDictionary[currentChunkPos].ToggleActive(false);   // Disable
                }
                else 
                {
                    chunkDictionary[currentChunkPos].ToggleActive(true);
                    chunkDictionary[currentChunkPos].UpdateChunk(detailLevel);    // Update
                }
            }
        }
    }

    private int CalculateDetailLevel(Vector2 currentChunkPos, Vector2 viewerCoord)
    {
        int detail = (int) (Mathf.Abs(currentChunkPos.x-viewerCoord.x) + Mathf.Abs(viewerCoord.y-currentChunkPos.y));
       
        
        if      (detail <= TerainSettings.Instance.lod_6) { return 6; }
        else if (detail <= TerainSettings.Instance.lod_5) { return 5; }
        else if (detail <= TerainSettings.Instance.lod_4) { return 4; }
        else if (detail <= TerainSettings.Instance.lod_3) { return 3; }
        else if (detail <= TerainSettings.Instance.lod_2) { return 2; }
        else if (detail <= TerainSettings.Instance.lod_1) { return 1; }
        else if (detail <= TerainSettings.Instance.lod_disable) { return -1; } // disable
        else if (detail >  TerainSettings.Instance.lod_delete) { return -2; } // delete
        return -1;
    }

}
