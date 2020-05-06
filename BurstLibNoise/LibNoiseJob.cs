using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Generator;
using System;

namespace BurstLibNoise
{
    /// <summary>
    /// A plane terrain density calculation job
    /// </summary>
    [BurstCompile]
    struct LibNoiseJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<ModuleData> moduleData;

        [WriteOnly] public NativeArray<float> heightmap;

        [ReadOnly] public int width;

        [ReadOnly] public int height;

        /// <summary>
        /// The execute method required for Unity's IJobParallelFor job type
        /// </summary>
        /// <param name="index">The iteration index provided by Unity's Job System</param>
        public void Execute(int index)
        {
            int positionX = (index / (width * height));
            int positionY = (index / width % height);
            int positionZ = (index % width);

            heightmap[index] = Calculate(positionX, positionY, positionZ);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Calculate(int x, int y, int z)
        {
            return BurstModuleManager.GetBurstValue(x, y, z, moduleData, 0);
        }
    }
}