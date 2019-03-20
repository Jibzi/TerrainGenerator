using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseMap
{

    private static float _limitNoiseScale = 0.00001f;
    
    public static float[,] GenerateNoiseMap(int noiseMapWidth, int noiseMapHeight, float noiseScale, int numOctaves, float lacunarity)
    {
        float[,] map = new float[noiseMapWidth, noiseMapHeight];
        
        //Make sure the scale is an appropriate value;
        if (noiseScale < _limitNoiseScale)
        {
            noiseScale = _limitNoiseScale;
        }

        
        
            for (int y = 0; y < noiseMapHeight; y++)
            {
                for (int x = 0; x < noiseMapWidth; x++)
                {
                    float value = 0f;
                    
                    for (int octave = 0; octave < numOctaves; octave++)
                    {
                        float sampleX = x / (noiseScale / (octave + 1));
                        float sampleY = y / (noiseScale / (octave + 1));

                        value += ((Mathf.PerlinNoise(sampleX, sampleY) * 2f) - 1f) / ((octave * lacunarity) + 1);
                        map[x, y] = value;
                    }

                }
            }
        


        return map;
    }
}
