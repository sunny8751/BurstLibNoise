using System;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using BurstLibNoise.Generator;
using BurstLibNoise.Operator;

namespace BurstLibNoise
{

    /// <summary>
    /// Base class for noise modules.
    /// </summary>
    public abstract class ModuleBase
    {

        /// <summary>
        /// Returns the output value for the given input coordinates.
        /// </summary>
        /// <param name="x">The input coordinate on the x-axis.</param>
        /// <param name="y">The input coordinate on the y-axis.</param>
        /// <param name="z">The input coordinate on the z-axis.</param>
        /// <returns>The resulting output value.</returns>
        public static float GetValue(float x, float y, float z, NativeArray<ModuleData> data, int dataIndex)
        {
            ModuleType type = data[dataIndex].type;
            switch (type)
            {
                // case ModuleType.Billow:
                //     return Billow.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Checker:
                //     return Checker.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Const:
                //     return Const.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Cyclinders:
                //     return Cyclinders.GetValue(x, y, z, data, dataIndex);
                case ModuleType.Perlin:
                    return Perlin.GetValue(x, y, z, data, dataIndex);
                case ModuleType.RidgedMultifractal:
                    return RidgedMultifractal.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Spheres:
                //     return Spheres.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Voronoi:
                //     return Voronoi.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Abs:
                //     return Abs.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Add:
                //     return Add.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Blend:
                //     return Blend.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Cache:
                //     return Cache.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Clamp:
                //     return Clamp.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Curve:
                //     return Curve.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Displace:
                //     return Displace.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Exponent:
                //     return Exponent.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Invert:
                //     return Invert.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Max:
                //     return Max.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Min:
                //     return Min.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Multiply:
                //     return Multiply.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Power:
                //     return Power.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Rotate:
                //     return Rotate.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Scale:
                //     return Scale.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.ScaleBias:
                //     return ScaleBias.GetValue(x, y, z, data, dataIndex);
                case ModuleType.Select:
                    return Select.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Subtract:
                //     return Subtract.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Terrace:
                //     return Terrace.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Translate:
                //     return Translate.GetValue(x, y, z, data, dataIndex);
                // case ModuleType.Turbulence:
                //     return Turbulence.GetValue(x, y, z, data, dataIndex);
                default:
                    // Debug.LogError("Not a valid module type");
                    return 0;
            }
        }

    }

    public enum ModuleType
    {
        Billow, Checker, Const, Cyclinders, Perlin, RidgedMultifractal, Spheres, Voronoi,
        Abs, Add, Blend, Cache, Clamp, Curve, Displace, Exponent, Invert, Max, Min, Multiply, Power, Rotate, Scale, ScaleBias, Select, Subtract, Terrace, Translate, Turbulence
    };

    public struct ModuleData
    {
        const int NUM_PARAMS = 5;

        public ModuleType type;
        public float param1;
        public float param2;
        public float param3;
        public float param4;
        public float param5;

        public ModuleData(ModuleType type, float param1 = 0, float param2 = 0, float param3 = 0, float param4 = 0, float param5 = 0)
        {
            this.type = type;
            this.param1 = param1;
            this.param2 = param2;
            this.param3 = param3;
            this.param4 = param4;
            this.param5 = param5;
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
                    default: throw new System.IndexOutOfRangeException();
                }
            }
            set
            {
                Debug.Assert(index >= 0 && index < NUM_PARAMS);
                switch (index)
                {
                    case 0:
                        param1 = value;
                        break;
                    case 1:
                        param2 = value;
                        break;
                    case 2:
                        param3 = value;
                        break;
                    case 3:
                        param4 = value;
                        break;
                    case 4:
                        param5 = value;
                        break;
                    default: throw new System.IndexOutOfRangeException();
                }
            }
        }
    }
}