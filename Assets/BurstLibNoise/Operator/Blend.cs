using System.Diagnostics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;

namespace BurstLibNoise.Operator
{
    public class Blend : LibNoise.Operator.Blend, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Blend, sources);
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
            // Debug.Assert(Modules[2] != null);
            var a = BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(0));
            var b = BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(1));
            var c = (BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(2)) + 1.0f) / 2.0f;
            return (float) Utils.InterpolateLinear(a, b, c);
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Blend.
        /// </summary>
        public Blend()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Blend.
        /// </summary>
        /// <param name="lhs">The left hand input module.</param>
        /// <param name="rhs">The right hand input module.</param>
        /// <param name="controller">The controller of the operator.</param>
        public Blend(ModuleBase lhs, ModuleBase rhs, ModuleBase controller)
            : base(lhs, rhs, controller)
        {
        }

        #endregion
    }
}