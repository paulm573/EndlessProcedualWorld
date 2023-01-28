using UnityEngine;
[System.Serializable]
public struct BiomeStruct{

    [SerializeField] public string biomeName;
    [SerializeField] public int biomeID;
    [SerializeField] public Color[] colors;
    [SerializeField] public int[] heightlevels;
    [SerializeField] public int blendStrengths;
    [SerializeField] public float colorVariation;

    public BiomeStruct(string biomeName, Color[] colors, int[] heightlevels,int biomeID, int blendStrengths, float colorVariation) {
        this.biomeName = biomeName;
        this.colors = colors;
        this.heightlevels = heightlevels;
        this.biomeID = biomeID;
        this.blendStrengths = blendStrengths;
        this.colorVariation = colorVariation;
    }

    public float rateLocation(float continentalness){
        if(continentalness > .5f) { return 1; }
        return 0;
    }
}