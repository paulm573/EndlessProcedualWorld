using UnityEngine;

public static class MeshPainter
{
    public static Color[] GenerateMeshColors(Vector3[] vertices, int detailLevel, int[] useLevels, Color[] biomeColors)
    {
        Color[] meshColors = new Color[vertices.Length];
        
        detailLevel = (detailLevel == 6) ? 1 : (detailLevel == 5) ? 2 : 12 / detailLevel;


        for (int i = 0; i < vertices.Length;)
        {
            int colorIndex = 0;
            float vertheight = vertices[i].y;
            for (int temp = 0; temp < useLevels.Length; temp++)
            {
                if (vertheight >= useLevels[temp]) 
                { 
                    colorIndex= temp;
                    break;
                }
            }
            
            meshColors[i] = biomeColors[colorIndex];
            i += detailLevel;
        }

        return meshColors;
    }


}