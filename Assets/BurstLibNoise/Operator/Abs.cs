using System.Diagnostics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;
using Unity.Mathematics;

namespace BurstLibNoise.Operator
{
    public class Abs : LibNoise.Operator.Abs, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Abs, sources);
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
            return math.abs(BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(0)));
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Abs.
        /// </summary>
        public Abs()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Abs.
        /// </summary>
        /// <param name="input">The input module.</param>
        public Abs(ModuleBase input)
            : base(input)
        {
        }

        #endregion
    }
}