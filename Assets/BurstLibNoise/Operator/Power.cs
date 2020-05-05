using System.Diagnostics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;
using Unity.Mathematics;

namespace BurstLibNoise.Operator
{
    public class Power : LibNoise.Operator.Power, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Power, sources);
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
            return math.pow(BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(0)), BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(1)));
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Power.
        /// </summary>
        public Power()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Power.
        /// </summary>
        /// <param name="lhs">The left hand input module.</param>
        /// <param name="rhs">The right hand input module.</param>
        public Power(ModuleBase lhs, ModuleBase rhs)
            : base(lhs, rhs)
        {
        }

        #endregion
    }
}