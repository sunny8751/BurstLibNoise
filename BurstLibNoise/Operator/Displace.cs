using System.Diagnostics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;

namespace BurstLibNoise.Operator
{
    public class Displace : LibNoise.Operator.Displace, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Displace, sources);
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
            // Debug.Assert(Modules[3] != null);
            var dx = x + BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(1));
            var dy = y + BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(2));
            var dz = z + BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(3));
            return BurstModuleManager.GetBurstValue(dx, dy, dz, data, moduleData.Source(0));
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Displace.
        /// </summary>
        public Displace()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Displace.
        /// </summary>
        /// <param name="input">The input module.</param>
        /// <param name="x">The displacement module of the x-axis.</param>
        /// <param name="y">The displacement module of the y-axis.</param>
        /// <param name="z">The displacement module of the z-axis.</param>
        public Displace(BurstModuleBase input, BurstModuleBase x, BurstModuleBase y, BurstModuleBase z)
            : base((ModuleBase) input, (ModuleBase) x, (ModuleBase) y, (ModuleBase) z)
        {
        }

        #endregion
    }
}