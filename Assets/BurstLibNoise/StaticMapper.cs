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

namespace BurstLibNoise.Manager
{
    public static class StaticMapper
    {
        public static float GetBurstValue(ModuleType type, float x, float y, float z, NativeArray<ModuleData> data, int dataIndex)
        {
            switch (type)
            {
                case ModuleType.Billow:
                    return Billow.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Checker:
                //     return Checker.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Const:
                //     return Const.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Cylinders:
                //     return Cylinders.GetBurstValue(x, y, z, data, dataIndex);
                case ModuleType.Perlin:
                    return Perlin.GetBurstValue(x, y, z, data, dataIndex);
                case ModuleType.RidgedMultifractal:
                    return RidgedMultifractal.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Spheres:
                //     return Spheres.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Voronoi:
                //     return Voronoi.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Abs:
                //     return Abs.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Add:
                //     return Add.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Blend:
                //     return Blend.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Cache:
                //     return Cache.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Clamp:
                //     return Clamp.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Curve:
                //     return Curve.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Displace:
                //     return Displace.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Exponent:
                //     return Exponent.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Invert:
                //     return Invert.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Max:
                //     return Max.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Min:
                //     return Min.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Multiply:
                //     return Multiply.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Power:
                //     return Power.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Rotate:
                //     return Rotate.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Scale:
                //     return Scale.GetBurstValue(x, y, z, data, dataIndex);
                case ModuleType.ScaleBias:
                    return ScaleBias.GetBurstValue(x, y, z, data, dataIndex);
                case ModuleType.Select:
                    return Select.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Subtract:
                //     return Subtract.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Terrace:
                //     return Terrace.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Translate:
                //     return Translate.GetBurstValue(x, y, z, data, dataIndex);
                // case ModuleType.Turbulence:
                //     return Turbulence.GetBurstValue(x, y, z, data, dataIndex);
                default:
                    // Debug.LogError("Not a valid module type");
                    return 0;
            }
        }

        // public static ModuleData GetModuleData(LibNoise.ModuleBase module, int[] sources)
        // {
        //     switch (module.GetType())
        //     {
        //         case typeof(LibNoise.Generator.Perlin):
        //             return Perlin.GetData((LibNoise.Generator.Perlin) module, sources);
        //         default:
        //             // Debug.LogError("Not a valid module type");
        //             // return ;
        //             throw new System.NotSupportedException();
        //     }
        // }
    }
}