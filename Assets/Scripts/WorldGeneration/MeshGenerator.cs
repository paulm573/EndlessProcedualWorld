using UnityEngine;

public static class MeshGenerator
{
    public static Mesh GenerateMeshFromHeightMap(float[,] heightMap,int amplitude,int detailLevel){
        Vector3[] vertices;
        int[] triangles;
        int meshSize = heightMap.GetLength(1);

        vertices = new Vector3[(meshSize) * (meshSize)];

     
        for (int z = 0, index = 0; z < meshSize; z++)
        {
            for (int x = 0; x < meshSize; x++)
            {
                float y;
                y = heightMap[x, z]*amplitude;
                vertices[index] = new Vector3(x, y, z);
                index++;
            }
        }

        detailLevel = (detailLevel == 6) ? 1 : (detailLevel == 5) ? 2:  12 / detailLevel; 


        int adjustedDimension = ((meshSize - 1) / detailLevel) + 1;
        triangles = new int[adjustedDimension * adjustedDimension * 6];

        int vertice = 0; //count up vertices
        int triangleIndex = 0;
        for (int z = 1; z < adjustedDimension; z++)
        {
            for (int x = 1; x < adjustedDimension; x++)
            {
                // 3....4   Vertice indices:
                // ......   |0-1| = |3-4| = detaillevel
                // 0....1   |0-3| = |1-4| = chunkSize*detaillevel
                // #1 -> 0,3,1
                triangles[0 + triangleIndex] = vertice; //bottom left -> 0
                triangles[1 + triangleIndex] = vertice + meshSize * detailLevel; //top left -> 3
                triangles[2 + triangleIndex] = vertice + detailLevel; // bottom right -> 1
                // #2 -> 3,4,1
                triangles[3 + triangleIndex] = vertice + meshSize * detailLevel; //top left -> 3
                triangles[4 + triangleIndex] = vertice + meshSize * detailLevel + detailLevel; // top right -> 4
                triangles[5 + triangleIndex] = vertice + detailLevel; // bottom right - 1

                vertice += detailLevel;
                triangleIndex += 6;
            }
            vertice = vertice + meshSize * detailLevel - meshSize + 1;
        }

        Mesh m = new Mesh();
        m.vertices = vertices;
        m.triangles = triangles;


        return m;
    }
}
