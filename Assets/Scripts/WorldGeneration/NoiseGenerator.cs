using UnityEngine;

public static class NoiseGenerator{
    
    public static float[,] Generate2DNoiseMap(int seed,int size, float scale, int octaves, float persistance, float lacunarity, Vector2 offset){
       
        float[,] noiseMap = new float[size,size];
      
        // SeedingSys
        System.Random rng = new System.Random(seed);
        Vector2[] octavesOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++){
            octavesOffsets[i] = new Vector2(rng.Next(-100000,100000)+offset.x,rng.Next(-100000,100000)+offset.y);
        }

        // Guard 0 division
        if(scale <= 0){scale = 0.00001f;}

        for (int z = 0; z < size; z++){
            for (int x = 0; x < size; x++){

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                float halfSize = size/2f;
                // layer Perlin noise 
                for (int i = 0; i < octaves; i++){
             
                    float sX, sY;
                    sX = ( (x-halfSize)  + octavesOffsets[i].x ) / scale * frequency;
                    sY = ( (z-halfSize)  + octavesOffsets[i].y ) / scale * frequency;

                    float perlinNoise = Mathf.PerlinNoise(sX,sY);
                    noiseHeight += perlinNoise *amplitude;

                    amplitude*= persistance;
                    frequency*= lacunarity;
                    
                }

                noiseMap[x,z] = noiseHeight;
            }
        }
        

        return noiseMap;
    }
}