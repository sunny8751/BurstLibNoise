using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using BurstLibNoise;
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
            ModuleData moduleData = data[dataIndex];
            float _frequency = moduleData[0];
            float _lacunarity = moduleData[1];
            float _persistence = moduleData[2];
            int _octaveCount = (int) moduleData[3];
            int _seed = (int) moduleData[4];

            return GetBurstValue(x, y, z, _frequency, _lacunarity, _persistence, _octaveCount, _seed);
        }

        // Special method for Turbulence operator
        public static float GetBurstValue(float x, float y, float z, float frequency, float lacunarity, float persistence, int octaveCount, int seed)
        {
            float value       = 0.0f;
            float amplitude   = 1.0f;

            x *= frequency;
            y *= frequency;
            z *= frequency;
            for (var i = 0; i < octaveCount; i++)
            {
                var nx = Utils.MakeInt32Range(x);
                var ny = Utils.MakeInt32Range(y);
                var nz = Utils.MakeInt32Range(z);
                int newSeed = seed + i;
                var signal = (float) Utils.GradientCoherentNoise3D(nx, ny, nz, newSeed);
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