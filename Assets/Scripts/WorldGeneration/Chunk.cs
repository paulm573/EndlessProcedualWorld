using UnityEngine;

public class Chunk
{
    
    private MeshRenderer meshRenderer;
    private GameObject chunk;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
  
    private Vector2 pos;
 
    private int currentDetailLevel;
    private bool biomeUnaplyed;

 
    public Chunk(Vector2 coordinates,int initialDetailLevel)
    {   
        currentDetailLevel= -100;
        biomeUnaplyed = true;


        pos = coordinates * (TerainSettings.Instance.chunkSize_*12);
        // Offset so player is centered
        pos.x -= TerainSettings.Instance.chunkSize_ * 6;
        pos.y -= TerainSettings.Instance.chunkSize_ * 6;
        chunk = new GameObject($"({coordinates.x}|{coordinates.y})");
        chunk.transform.position = new Vector3(pos.x, 0, pos.y);

        meshRenderer = chunk.AddComponent<MeshRenderer>();
        meshFilter = chunk.AddComponent<MeshFilter>();
        meshCollider = chunk.AddComponent<MeshCollider>();

        chunk.transform.parent = TerainSettings.Instance.worldRoot;

        

        UpdateChunk(initialDetailLevel);

    }

    public void UpdateChunk(int desiredDetailLevel)
    {
        // check if chunk is already up to date
        if (desiredDetailLevel == currentDetailLevel) { return; } 

        currentDetailLevel = desiredDetailLevel;
        ChunkBuilderSingelton.Instance.RequestChunkData(OnDataReceived, pos, desiredDetailLevel);

    }

    public void SelfDestroy()
    {
        Object.Destroy(chunk.gameObject);
        Object.Destroy(chunk);
    }

    private void OnDataReceived(ChunkInfo chunkInfo)
    {
        // UpdateBiome
        if (biomeUnaplyed) {
            meshRenderer.material = TerainSettings.Instance.biomes[chunkInfo.biomeID].getBiomeMaterial();
            biomeUnaplyed = false;
        }
        // UpdateMesh
        meshFilter.mesh.vertices = chunkInfo.vertices;
        meshFilter.mesh.triangles = chunkInfo.triangles;
        
        if (currentDetailLevel >= 5)
        {
            meshCollider.sharedMesh = meshFilter.sharedMesh;
        }
        else {
            meshCollider.sharedMesh = null;
        }

        meshFilter.mesh.RecalculateNormals();  
    }

    public void ToggleActive(bool v)
    {
        chunk.SetActive(v);
    }
}


public struct ChunkInfo
{
    public readonly Vector3[] vertices;
    public readonly int[] triangles;
    public readonly int biomeID;
    

    public ChunkInfo(Vector3[] verts, int[] tris,int biomeID)
    {   
        this.biomeID = biomeID;
        this.vertices = verts;
        this.triangles = tris;

    }
}
