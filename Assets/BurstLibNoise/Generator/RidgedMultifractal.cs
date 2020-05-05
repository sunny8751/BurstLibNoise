using System;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using BurstLibNoise;
using LibNoise;

namespace BurstLibNoise.Generator
{
    /// <summary>
    /// Provides a noise module that outputs 3-dimensional ridged-multifractal noise. [GENERATOR]
    /// </summary>
    public class RidgedMultifractal : LibNoise.Generator.RidgedMultifractal, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.RidgedMultifractal, sources, (float) Frequency, (float) Lacunarity, OctaveCount, Seed);
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
            int _octaveCount = (int) moduleData[2];
            int _seed = (int) moduleData[3];
            
            float exponent = 1.0f;
            float gain = 2.0f;
            float offset = 1.0f;

            float value   = 0.0f;
            float weight  = 1.0f;

            x *= _frequency;
            y *= _frequency;
            z *= _frequency;
            var f = 1.0f;
            for (var i = 0; i < _octaveCount; i++)
            {
                int seed   = _seed + i;
                var signal  = Utils.GradientCoherentNoise3D(x, y, z, seed);
                signal = math.abs(signal);
                signal = offset - signal;
                signal *= signal;
                signal *= weight;
                weight = signal * gain;
                weight = math.clamp((float) weight, 0, 1);

                value += (signal * math.pow(f, -exponent));
                f *= _lacunarity;

                x *= _lacunarity;
                y *= _lacunarity;
                z *= _lacunarity;
            }
            return (value * 1.25f) - 1.0f;
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of RidgedMultifractal.
        /// </summary>
        public RidgedMultifractal()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of RidgedMultifractal.
        /// </summary>
        /// <param name="frequency">The frequency of the first octave.</param>
        /// <param name="lacunarity">The lacunarity of the ridged-multifractal noise.</param>
        /// <param name="octaves">The number of octaves of the ridged-multifractal noise.</param>
        /// <param name="seed">The seed of the ridged-multifractal noise.</param>
        /// <param name="quality">The quality of the ridged-multifractal noise.</param>
        public RidgedMultifractal(double frequency, double lacunarity, int octaves, int seed, QualityMode quality)
            : base(frequency, lacunarity, octaves, seed, quality)
        {
        }

        #endregion
    }
}