using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;

namespace BurstLibNoise.Generator
{
    public class Billow : LibNoise.Generator.Billow, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Billow, sources, (float) Frequency, (float) Lacunarity, (float) Persistence, OctaveCount, Seed);
        }

        public static BurstModuleBase ParseData(ModuleData[] moduleData, ref ModuleData data) {
            return new Billow(data[0], data[1], data[2], (int) data[3], (int) data[4], QualityMode.Medium);
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

            var value       = 0.0f;
            var amplitude   = 1.0f;

            x *= _frequency;
            y *= _frequency;
            z *= _frequency;
            for (var i = 0; i < _octaveCount; i++)
            {
                var nx = Utils.MakeInt32Range(x);
                var ny = Utils.MakeInt32Range(y);
                var nz = Utils.MakeInt32Range(z);
                var seed = (_seed + i);
                var signal = (float) Utils.GradientCoherentNoise3D(nx, ny, nz, seed);
                signal = 2.0f * math.abs(signal) - 1.0f;
                value += signal * amplitude;
                x *= _lacunarity;
                y *= _lacunarity;
                z *= _lacunarity;
                amplitude *= _persistence;
            }
            return value + 0.5f;
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Billow.
        /// </summary>
        public Billow()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Billow.
        /// </summary>
        /// <param name="frequency">The frequency of the first octave.</param>
        /// <param name="lacunarity">The lacunarity of the billowy noise.</param>
        /// <param name="persistence">The persistence of the billowy noise.</param>
        /// <param name="octaves">The number of octaves of the billowy noise.</param>
        /// <param name="seed">The seed of the billowy noise.</param>
        /// <param name="quality">The quality of the billowy noise.</param>
        public Billow(double frequency, double lacunarity, double persistence, int octaves, int seed,
            QualityMode quality)
            : base(frequency, lacunarity, persistence, octaves, seed, quality)
        {
        }

        #endregion
    }
}