using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using BurstLibNoise.Manager;

namespace BurstLibNoise.Generator
{
    /// <summary>
    /// Provides a noise module that outputs a three-dimensional perlin noise. [GENERATOR]
    /// </summary>
    public static class Perlin
    {
        public static ModuleData GetData(LibNoise.Generator.Perlin perlin, int[] sources) {
            return new ModuleData(ModuleType.Perlin, sources, (float) perlin.Frequency, (float) perlin.Lacunarity, (float) perlin.Persistence, perlin.OctaveCount, perlin.Seed);
        }

        /// <summary>
        /// Returns the output value for the given input coordinates.
        /// </summary>
        /// <param name="x">The input coordinate on the x-axis.</param>
        /// <param name="y">The input coordinate on the y-axis.</param>
        /// <param name="z">The input coordinate on the z-axis.</param>
        /// <returns>The resulting output value.</returns>
        public static float GetBurstValue(float x, float y, float z, NativeArray<ModuleData> data, int dataIndex)
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