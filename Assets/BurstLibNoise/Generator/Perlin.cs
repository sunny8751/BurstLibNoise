using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using BurstLibNoise.Manager;
using LibNoise;

namespace BurstLibNoise.Generator
{
    /// <summary>
    /// Provides a noise module that outputs a three-dimensional perlin noise. [GENERATOR]
    /// </summary>
    public class Perlin : LibNoise.Generator.Perlin, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Perlin, sources, (float) Frequency, (float) Lacunarity, (float) Persistence, OctaveCount, Seed);
        }

        // Must be included in each file because Unity does not support C# 8.0 not supported yet (default interface implementation)
        public BurstModuleBase Source(int i) {
            return (BurstModuleBase) Modules[i];
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

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Perlin.
        /// </summary>
        public Perlin()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Perlin.
        /// </summary>
        /// <param name="frequency">The frequency of the first octave.</param>
        /// <param name="lacunarity">The lacunarity of the perlin noise.</param>
        /// <param name="persistence">The persistence of the perlin noise.</param>
        /// <param name="octaves">The number of octaves of the perlin noise.</param>
        /// <param name="seed">The seed of the perlin noise.</param>
        /// <param name="quality">The quality of the perlin noise.</param>
        public Perlin(double frequency, double lacunarity, double persistence, int octaves, int seed,
            QualityMode quality)
            : base(frequency, lacunarity, persistence, octaves, seed, quality)
        {
        }

        #endregion
    }
}