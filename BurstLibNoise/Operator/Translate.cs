using System.Diagnostics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;

namespace BurstLibNoise.Operator
{
    public class Translate : LibNoise.Operator.Translate, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Translate, sources, (float) X, (float) Y, (float) Z);
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
            float _x = moduleData[0];
            float _y = moduleData[1];
            float _z = moduleData[2];
            
            // Debug.Assert(Modules[0] != null);
            return BurstModuleManager.GetBurstValue(x + _x, y + _y, z + _z, data, moduleData.Source(0));
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Translate.
        /// </summary>
        public Translate()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Translate.
        /// </summary>
        /// <param name="input">The input module.</param>
        public Translate(BurstModuleBase input)
            : base((ModuleBase) input)
        {
        }

        /// <summary>
        /// Initializes a new instance of Translate.
        /// </summary>
        /// <param name="x">The translation on the x-axis.</param>
        /// <param name="y">The translation on the y-axis.</param>
        /// <param name="z">The translation on the z-axis.</param>
        /// <param name="input">The input module.</param>
        public Translate(double x, double y, double z, BurstModuleBase input)
            : base(x, y, z, (ModuleBase) input)
        {
        }

        #endregion
    }
}