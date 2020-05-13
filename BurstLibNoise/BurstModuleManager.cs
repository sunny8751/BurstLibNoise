using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using LibNoise;
using Unity.Jobs;

namespace BurstLibNoise
{
    /// <summary>
    /// Base class for noise modules.
    /// </summary>
    public class BurstModuleManager
    {
        public static void GeneratePlanarHeightmap(NativeArray<float> heightmap, BurstModuleBase module, int width, int height, double left, double right, double top, double bottom, bool isSeamless) {
            NativeArray<ModuleData> moduleData = CreateModuleData(module);
            GenerateHeightmap(heightmap, moduleData, GenerateMode.Planar, width, height, left, right, top, bottom, isSeamless);
        }

        public static void GenerateCylindricalHeightmap(NativeArray<float> heightmap, BurstModuleBase module, int width, int height, double angleMin, double angleMax, double heightMin, double heightMax) {
            NativeArray<ModuleData> moduleData = CreateModuleData(module);
            GenerateHeightmap(heightmap, moduleData, GenerateMode.Cylindrical, width, height, angleMin, angleMax, heightMin, heightMax);
        }

        public static void GenerateSphericalHeightmap(NativeArray<float> heightmap, BurstModuleBase module, int width, int height, double south, double north, double west, double east) {
            NativeArray<ModuleData> moduleData = CreateModuleData(module);
            GenerateHeightmap(heightmap, moduleData, GenerateMode.Spherical, width, height, south, north, west, east);
        }

        public static void GeneratePlanarHeightmap(NativeArray<float> heightmap, NoiseSettings noiseSettings, int width, int height, double left, double right, double top, double bottom, bool isSeamless) {
            NativeArray<ModuleData> moduleData = CreateModuleData(noiseSettings);
            GenerateHeightmap(heightmap, moduleData, GenerateMode.Planar, width, height, left, right, top, bottom, isSeamless);
        }

        public static void GenerateCylindricalHeightmap(NativeArray<float> heightmap, NoiseSettings noiseSettings, int width, int height, double angleMin, double angleMax, double heightMin, double heightMax) {
            NativeArray<ModuleData> moduleData = CreateModuleData(noiseSettings);
            GenerateHeightmap(heightmap, moduleData, GenerateMode.Cylindrical, width, height, angleMin, angleMax, heightMin, heightMax);
        }

        public static void GenerateSphericalHeightmap(NativeArray<float> heightmap, NoiseSettings noiseSettings, int width, int height, double south, double north, double west, double east) {
            NativeArray<ModuleData> moduleData = CreateModuleData(noiseSettings);
            GenerateHeightmap(heightmap, moduleData, GenerateMode.Spherical, width, height, south, north, west, east);
        }

        private static void GenerateHeightmap(NativeArray<float> heightmap, NativeArray<ModuleData> moduleData, GenerateMode generateMode, int width, int height, double p1, double p2, double p3, double p4, bool p5 = false) {
            var job = new GenerateLibNoiseJob
            {
                moduleData = moduleData,
                heightmap = heightmap,
                width = width,
                height = height,
                generateMode = generateMode,
                p1 = p1,
                p2 = p2,
                p3 = p3,
                p4 = p4,
                p5 = p5
            };
            JobHandle jobHandle = job.Schedule(heightmap.Length, 256);
            jobHandle.Complete();

            // // create texture
            // Texture2D texture = new Texture2D(Width, Height);
            // float[] heightmapValues = heightmap.ToArray();
            // Color[] colors = new Color[heightmapValues.Length];
            // for (int i = 0; i < heightmapValues.Length; i++) {
            //     float value = heightmapValues[i];
            //     value = (value + 1) / 2;
            //     colors[i] = new Color(value, value, value, 1);
            // }
            // texture.SetPixels(colors);
            // texture.wrapMode = TextureWrapMode.Clamp;
            // texture.Apply();

            // GetComponent<Renderer>().material.mainTexture = texture;

            moduleData.Dispose();
        }

        public static NativeArray<ModuleData> CreateModuleData(BurstModuleBase module) {
            List<ModuleData> modules = new List<ModuleData>();
            CreateModuleDataHelper(modules, module);
            return Array2NativeArray(modules.ToArray());
        }

        private static int CreateModuleDataHelper(List<ModuleData> modules, BurstModuleBase module) {
            // Add with pre-order DFS
            if (module == null) {
                return -1;
            }
            modules.Add(new ModuleData(0, new int[]{}));
            int index = modules.Count - 1;
            int[] sources = new int[((ModuleBase) module).SourceModuleCount];
            for (int i = 0; i < sources.Length; i++) {
                sources[i] = CreateModuleDataHelper(modules, module.Source(i)); // add one for the current module
            }
            modules[index] = module.GetData(sources);
            return index;
        }

        public static BurstModuleBase ParseModuleData(ModuleData[] moduleData) {
            Debug.Assert(moduleData.Length > 0);
            return StaticMapper.ParseModuleData(moduleData, ref moduleData[0]);
        }

        public static NativeArray<ModuleData> CreateModuleData(NoiseSettings noiseSettings) {
            return Array2NativeArray(noiseSettings.moduleData);
        }

        public static NativeArray<ModuleData> Array2NativeArray(ModuleData[] modules) {
            NativeArray<ModuleData> moduleData = new NativeArray<ModuleData>(modules.Length, Allocator.Persistent);
            // TODO replace with unsafe mem copy
            for (int i = 0; i < modules.Length; i++) {
                moduleData[i] = modules[i];
            }
            return moduleData;
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
            ModuleType type = data[dataIndex].type;
            return StaticMapper.GetBurstValue(type, x, y, z, data, dataIndex);
        }
    }

    public enum GenerateMode {
        Planar, Cylindrical, Spherical
    };

    public enum ModuleType
    {
        Billow, Checker, Const, Cylinders, Perlin, RidgedMultifractal, Spheres, Voronoi,
        Abs, Add, Blend, Cache, Clamp, Curve, Displace, Exponent, Invert, Max, Min, Multiply, Power, Rotate, Scale, ScaleBias, Select, Subtract, Terrace, Translate, Turbulence
    };

    [System.Serializable]
    public struct ModuleData
    {
        private const int NUM_PARAMS = 6;

        public ModuleType type;
        public float param1;
        public float param2;
        public float param3;
        public float param4;
        public float param5;
        public float param6;

        public int source1;
        public int source2;
        public int source3;
        public int source4;

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