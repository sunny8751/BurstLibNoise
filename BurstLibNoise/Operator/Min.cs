using System.Diagnostics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;
using Unity.Mathematics;

namespace BurstLibNoise.Operator
{
    public class Min : LibNoise.Operator.Min, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Min, sources);
        }

        public static BurstModuleBase ParseData(ModuleData[] moduleData, ref ModuleData data) {
            return new Min(StaticMapper.ParseModuleData(moduleData, ref moduleData[data.source1]),
                StaticMapper.ParseModuleData(moduleData, ref moduleData[data.source2]));
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
            
            // Debug.Assert(Modules[0] != null);
            // Debug.Assert(Modules[1] != null);
            var a = BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(0));
            var b = BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(1));
            return math.min(a, b);
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Min.
        /// </summary>
        public Min()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Min.
        /// </summary>
        /// <param name="lhs">The left hand input module.</param>
        /// <param name="rhs">The right hand input module.</param>
        public Min(BurstModuleBase lhs, BurstModuleBase rhs)
            : base((ModuleBase) lhs, (ModuleBase) rhs)
        {
        }

        #endregion
    }
}