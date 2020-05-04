using System;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

namespace BurstLibNoise.Generator
{
    /// <summary>
    /// Provides a noise module that outputs 3-dimensional ridged-multifractal noise. [GENERATOR]
    /// </summary>
    public class RidgedMultifractal
    {
        public static ModuleData GetData(float frequency=1.0f, float lacunarity=2.0f, int octaves=6, int seed=0) {
            return new ModuleData(ModuleType.RidgedMultifractal, frequency, lacunarity, octaves, seed);
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
            int octaves = (int) perlinData[2];
            int seed = (int) perlinData[3];
            
            float exponent = 1.0f;
            float gain = 2.0f;
            float offset = 1.0f;

            float value   = 0.0f;
            float weight  = 1.0f;

            x *= frequency;
            y *= frequency;
            z *= frequency;
            var f = 1.0f;
            for (var i = 0; i < octaves; i++)
            {
                int newSeed   = seed + i;
                var signal  = Utils.GradientCoherentNoise3D(x, y, z, newSeed);
                signal = math.abs(signal);
                signal = offset - signal;
                signal *= signal;
                signal *= weight;
                weight = signal * gain;
                weight = math.clamp((float) weight, 0, 1);

                value += (signal * math.pow(f, -exponent));
                f *= lacunarity;

                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
            }
            return (value * 1.25f) - 1.0f;
        }
    }
}