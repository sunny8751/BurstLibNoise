using System.Diagnostics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;
using Unity.Mathematics;

namespace BurstLibNoise.Operator
{
    public class Exponent : LibNoise.Operator.Exponent, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Exponent, sources, (float) Value);
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
            float _exponent = moduleData[0];
            
            // Debug.Assert(Modules[0] != null);
            var v = BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(0));
            return (math.pow(math.abs((v + 1.0f) / 2.0f), _exponent) * 2.0f - 1.0f);
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Exponent.
        /// </summary>
        public Exponent()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Exponent.
        /// </summary>
        /// <param name="input">The input module.</param>
        public Exponent(ModuleBase input)
            : base(input)
        {
        }

        /// <summary>
        /// Initializes a new instance of Exponent.
        /// </summary>
        /// <param name="exponent">The exponent to use.</param>
        /// <param name="input">The input module.</param>
        public Exponent(double exponent, ModuleBase input)
            : base(exponent, input)
        {
        }

        #endregion
    }
}