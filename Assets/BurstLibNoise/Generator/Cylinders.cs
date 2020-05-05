using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;

namespace BurstLibNoise.Generator
{
    public class Cylinders : LibNoise.Generator.Cylinders, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Cylinders, sources, (float) Frequency);
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
            float _frequency = moduleData[0];
            
            x *= _frequency;
            z *= _frequency;
            var dfc = math.sqrt(x * x + z * z);
            var dfss = dfc - math.floor(dfc);
            var dfls = 1.0f - dfss;
            var nd = math.min(dfss, dfls);
            return 1.0f - (nd * 4.0f);
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Cylinders.
        /// </summary>
        public Cylinders()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Cylinders.
        /// </summary>
        /// <param name="frequency">The frequency of the concentric cylinders.</param>
        public Cylinders(double frequency)
            : base(frequency)
        {
        }

        #endregion
    }
}