using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;

namespace BurstLibNoise.Generator
{
    public class Const : LibNoise.Generator.Const, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Const, sources, (float) Value);
        }

        public static BurstModuleBase ParseData(ModuleData[] moduleData, ref ModuleData data) {
            return new Const(data[0]);
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
            float _value = moduleData[0];
            return _value;
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Const.
        /// </summary>
        public Const()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Const.
        /// </summary>
        /// <param name="value">The constant value.</param>
        public Const(double value)
            : base(value)
        {
        }

        #endregion
    }
}