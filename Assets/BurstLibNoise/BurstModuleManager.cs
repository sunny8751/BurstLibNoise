using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using BurstLibNoise;
using BurstLibNoise.Generator;
using BurstLibNoise.Operator;

namespace BurstLibNoise
{
    /// <summary>
    /// Base class for noise modules.
    /// </summary>
    public static class BurstModuleManager
    {

        /// <summary>
        /// Returns the output value for the given input coordinates.
        /// </summary>
        /// <param name="x">The input coordinate on the x-axis.</param>
        /// <param name="y">The input coordinate on the y-axis.</param>
        /// <param name="z">The input coordinate on the z-axis.</param>
        /// <returns>The resulting output value.</returns>
        public static float GetBurstValue(float x, float y, float z, NativeArray<ModuleData> data, int dataIndex)
        {
            ModuleType type = data[dataIndex].type;
            return StaticMapper.GetBurstValue(type, x, y, z, data, dataIndex);
        }

    }

    public enum ModuleType
    {
        Billow, Checker, Const, Cylinders, Perlin, RidgedMultifractal, Spheres, Voronoi,
        Abs, Add, Blend, Cache, Clamp, Curve, Displace, Exponent, Invert, Max, Min, Multiply, Power, Rotate, Scale, ScaleBias, Select, Subtract, Terrace, Translate, Turbulence
    };

    public struct ModuleData
    {
        private const int NUM_PARAMS = 6;

        public readonly ModuleType type;
        public readonly float param1;
        public readonly float param2;
        public readonly float param3;
        public readonly float param4;
        public readonly float param5;
        public readonly float param6;

        public readonly int source1;
        public readonly int source2;
        public readonly int source3;
        public readonly int source4;

        public ModuleData(ModuleType type, int[] sources, float param1 = 0, float param2 = 0, float param3 = 0, float param4 = 0, float param5 = 0, float param6 = 0)
        {
            this.type = type;
            this.param1 = param1;
            this.param2 = param2;
            this.param3 = param3;
            this.param4 = param4;
            this.param5 = param5;
            this.param6 = param6;

            this.source1 = -1;
            this.source2 = -1;
            this.source3 = -1;
            this.source4 = -1;
            switch (sources.Length) {
                case 4:
                    this.source4 = sources[3];
                    goto case 3;
                case 3:
                    this.source3 = sources[2];
                    goto case 2;
                case 2:
                    this.source2 = sources[1];
                    goto case 1;
                case 1:
                    this.source1 = sources[0];
                    break;
                case 0:
                    break;
                default: throw new System.NotSupportedException(); // can't have more than 4 sources
            }
        }

        public int Source(int index) {
            switch (index)
            {
                case 0: return source1;
                case 1: return source2;
                case 2: return source3;
                case 3: return source4;
                default: throw new System.IndexOutOfRangeException();
            }
        }

        public float this[int index]
        {
            get
            {
                Debug.Assert(index >= 0 && index < NUM_PARAMS);
                switch (index)
                {
                    case 0: return param1;
                    case 1: return param2;
                    case 2: return param3;
                    case 3: return param4;
                    case 4: return param5;
                    case 5: return param6;
                    default: throw new System.IndexOutOfRangeException();
                }
            }
        }
    }
}