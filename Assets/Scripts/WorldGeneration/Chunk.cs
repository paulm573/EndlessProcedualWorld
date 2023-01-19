using UnityEngine;

public class Chunk
{
    
    private MeshRenderer meshRenderer;
    private GameObject chunk;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
  
    private Vector2 pos;
    private Vector2 coordinates;
 
    private int currentDetailLevel;

    private bool alive;
    public Chunk(Vector2 coordinates,int initialDetailLevel)
    {   
        currentDetailLevel= -100;
        alive = true;

        this.coordinates = coordinates;
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

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        meshRenderer.material = TerainSettings.Instance.placeholderMat;
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

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
        // UpdateMesh
        meshFilter.mesh.vertices = chunkInfo.vertices;
        meshFilter.mesh.triangles = chunkInfo.triangles;
        meshFilter.mesh.RecalculateNormals();

        // Colider
        if (chunkInfo.useColider)
        {
            meshCollider.sharedMesh = meshFilter.sharedMesh;
        }
        else 
        {
            meshCollider.sharedMesh = null;
        }
    
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
    public readonly bool useColider;

    public ChunkInfo(Vector3[] verts, int[] tris, bool colider)
    {   
        this.useColider  = colider;
        this.vertices = verts;
        this.triangles = tris;
    }
}
