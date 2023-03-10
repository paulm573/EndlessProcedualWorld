
using UnityEngine;


public class Chunk
{
    
    private MeshRenderer meshRenderer;
    private GameObject chunk;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
  
    private Vector2 pos;
    private bool isDead = false;
  
 
    private int currentDetailLevel;

    
    public Chunk(Vector2 coordinates,int initialDetailLevel)
    {   
        currentDetailLevel= -100;
     

       
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
        if (desiredDetailLevel ==currentDetailLevel) { return; } 

        currentDetailLevel = desiredDetailLevel;
        ChunkBuilderSingelton.Instance.RequestChunkData(OnDataReceived, pos, desiredDetailLevel);
       

    }

    public void Destroy()
    {
        isDead = true;
        Object.Destroy(chunk.gameObject);
        Object.Destroy(chunk);
    }

    private void OnDataReceived(ChunkInfo chunkInfo)
    {
        if(isDead) return;

        // UpdateMesh
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.vertices = chunkInfo.vertices;
        meshFilter.mesh.triangles = chunkInfo.triangles;

        meshFilter.mesh.colors = chunkInfo.colors;
        // Colider
        if (currentDetailLevel == 6) { meshCollider.sharedMesh = meshFilter.sharedMesh; } else { meshCollider.sharedMesh = null; }
            
                
        

        //meshFilter.mesh.RecalculateNormals();
        //meshFilter.mesh.RecalculateTangents();
     
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
    public readonly Color[] colors;

    public ChunkInfo(Vector3[] verts, int[] tris, Color[] colors)
    {
        this.vertices = verts;
        this.triangles = tris;
        this.colors = colors;
    }
}
