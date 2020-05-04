using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

namespace BurstLibNoise.Generator
{
    /// <summary>
    /// Provides a noise module that outputs a three-dimensional perlin noise. [GENERATOR]
    /// </summary>
    public class Perlin
    {
        public static ModuleData GetData(float frequency=1.0f, float lacunarity=2.0f, float persistence=0.5f, int octaves=6, int seed=0) {
            return new ModuleData(ModuleType.Perlin, frequency, lacunarity, persistence, octaves, seed);
        }

        /// <summary>
        /// Returns the output value for the given input coordinates.
        /// </summary>
        /// <param name="x">The input coordinate on the x-axis.</param>
        /// <param name="y">The input coordinate on the y-axis.</param>
        /// <param name="z">The input coordinate on the z-axis.</param>
        /// <returns>The resulting output value.</returns>
        public static float GetValue(float x, float y, float z, NativeArray<ModuleData> data, int dataIndex)
        {
            ModuleData perlinData = data[dataIndex];
            float frequency = perlinData[0];
            float lacunarity = perlinData[1];
            float persistence = perlinData[2];
            int octaves = (int) perlinData[3];
            int seed = (int) perlinData[4];

            float value       = 0.0f;
            float amplitude   = 1.0f;

            x *= frequency;
            y *= frequency;
            z *= frequency;
            for (var i = 0; i < octaves; i++)
            {
                int newSeed = seed + i;

                // System.Random random = new System.Random(seed);
                // var signal = noise.snoise(new float3(x + random.Next(1000), y + random.Next(1000), z + random.Next(1000)));
                // var signal = noise.snoise(new float3(x, y, z) + Utils.RandomOffset(newSeed));
                var signal = Utils.GradientCoherentNoise3D(x, y, z, newSeed);
                value += signal * amplitude;
                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
                amplitude *= persistence;
            }
            return value;
        }
    }
}