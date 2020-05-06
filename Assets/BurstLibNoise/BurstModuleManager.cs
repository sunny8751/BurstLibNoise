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
        public void GenerateHeightmap(NativeArray<float> heightmap, BurstModuleBase module, GenerateMode generateMode, int width, int height, double p1, double p2, double p3, double p4, bool p5 = false) {
            List<ModuleData> modules = GetModuleData(module);

            // TODO replace with unsafe mem copy
            NativeArray<ModuleData> moduleData = new NativeArray<ModuleData>(modules.Count, Allocator.Persistent);
            for (int i = 0; i < modules.Count; i++) {
                moduleData[i] = modules[i];
            }

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
            // // Debug.Log(colors[0]);
            // // Debug.Log(colors[5]);
            // texture.SetPixels(colors);
            // texture.wrapMode = TextureWrapMode.Clamp;
            // texture.Apply();

            // GetComponent<Renderer>().material.mainTexture = texture;

            moduleData.Dispose();
        }

        private List<ModuleData> GetModuleData(BurstModuleBase root) {
            List<ModuleData> modules = new List<ModuleData>();
            Queue<BurstModuleBase> queue = new Queue<BurstModuleBase>();
            queue.Enqueue(root);

            while (queue.Count > 0) {
                BurstModuleBase module = queue.Dequeue();
                int[] sourceIndices = new int[((ModuleBase) module).SourceModuleCount];
                for (int i = 0; i < sourceIndices.Length; i++) {
                    queue.Enqueue(module.Source(i));
                    sourceIndices[i] = modules.Count + i + 1; // add one for the current module
                }
                modules.Add(module.GetData(sourceIndices));
                // Debug.Log(module.GetData(sourceIndices).type);
            }
            return modules;
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