using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;

namespace BurstLibNoise.Generator
{
    public class Checker : LibNoise.Generator.Checker, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Checker, sources);
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
            var ix = (int) (math.floor(x));
            var iy = (int) (math.floor(y));
            var iz = (int) (math.floor(z));
            return (ix & 1 ^ iy & 1 ^ iz & 1) != 0 ? -1.0f : 1.0f;
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Checker.
        /// </summary>
        public Checker()
            : base()
        {
        }

        #endregion
    }
}