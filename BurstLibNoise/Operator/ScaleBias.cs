using System.Diagnostics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;

namespace BurstLibNoise.Operator
{
    public class ScaleBias : LibNoise.Operator.ScaleBias, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.ScaleBias, sources, (float) Scale, (float) Bias);
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
            float _scale = moduleData[0];
            float _bias = moduleData[1];
            
            return BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(0)) * _scale + _bias;
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of ScaleBias.
        /// </summary>
        public ScaleBias()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of ScaleBias.
        /// </summary>
        /// <param name="input">The input module.</param>
        public ScaleBias(BurstModuleBase input)
            : base((ModuleBase) input)
        {
        }

        /// <summary>
        /// Initializes a new instance of ScaleBias.
        /// </summary>
        /// <param name="scale">The scaling factor to apply to the output value from the source module.</param>
        /// <param name="bias">The bias to apply to the scaled output value from the source module.</param>
        /// <param name="input">The input module.</param>
        public ScaleBias(double scale, double bias, BurstModuleBase input)
            : base(scale, bias, (ModuleBase) input)
        {
        }

        #endregion
    }
}