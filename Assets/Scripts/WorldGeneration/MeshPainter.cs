using UnityEngine;

public static class MeshPainter
{
    public static Color[] GenerateMeshColors(int seed,Vector3[] vertices, int detailLevel, int[] useLevels,int blend, Color[] biomeColors,float variation)
    {
        Color[] meshColors = new Color[vertices.Length];
        System.Random rng = new System.Random(seed);

        ///detailLevel = (detailLevel == 6) ? 1 : (detailLevel == 5) ? 2 : 12 / detailLevel;
        // For Flatshading:
        detailLevel = 1;


        int i = 0;
        double max, min;
        max = variation;
        min = -max;
        do
        {
            int colorIndex = 0;
            float vertheight = vertices[i].y;
            for (int temp = 0; temp < useLevels.Length; temp++)
            {
                if (vertheight + rng.Next(-blend, 0) >= useLevels[temp])
                {
                    colorIndex = temp;
                    break;
                }
            }

            float[] off = { (float)(rng.NextDouble() * (max - min) + min), (float)(rng.NextDouble() * (max - min) + min), (float)(rng.NextDouble() * (max - min) + min) };
            meshColors[i] = new Color(biomeColors[colorIndex].r + off[0], biomeColors[colorIndex].g + off[0], biomeColors[colorIndex].b + off[0]);




            i += detailLevel;
        }
        while (i < vertices.Length);

        return meshColors;
    }


}