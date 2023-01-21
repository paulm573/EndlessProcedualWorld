using UnityEngine;

public static class MeshPainter
{
    public static Color[] GenerateMeshColors(int seed,Vector3[] vertices, int detailLevel, int[] useLevels,int shift, Color[] biomeColors)
    {
        Color[] meshColors = new Color[vertices.Length];
        System.Random rng = new System.Random(seed);

        detailLevel = (detailLevel == 6) ? 1 : (detailLevel == 5) ? 2 : 12 / detailLevel;

        int i = 0;
        do
        {
            int colorIndex = 0;
            float vertheight = vertices[i].y;
            for (int temp = 0; temp < useLevels.Length; temp++)
            {
                if (vertheight + rng.Next(-20,0) >= useLevels[temp] + shift)
                {
                    colorIndex = temp * 2;
                    break;
                }
            }

            meshColors[i] = biomeColors[colorIndex];
            i += detailLevel;
        }
        while (i < vertices.Length);

        return meshColors;
    }


}