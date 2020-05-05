using System.Diagnostics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;

namespace BurstLibNoise.Operator
{
    /// <summary>
    /// Provides a noise module that outputs the value selected from one of two source
    /// modules chosen by the output value from a control module. [OPERATOR]
    /// </summary>
    public class Select : LibNoise.Operator.Select, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Select, sources, (float) Minimum, (float) Maximum, (float) FallOff);
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
            // Debug.Assert(dataIndex + 3 < data.Length);
            ModuleData moduleData = data[dataIndex];
            float _min = moduleData[0];
            float _max = moduleData[1];
            float _fallOff = moduleData[2];
            
            float cv = BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(2));
            if (_fallOff > 0.0)
            {
                float a;
                if (cv < (_min - _fallOff))
                {
                    return BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(0));
                }
                if (cv < (_min + _fallOff))
                {
                    var lc = (_min - _fallOff);
                    var uc = (_min + _fallOff);
                    a = Utils.MapCubicSCurve((cv - lc) / (uc - lc));
                    return Utils.InterpolateLinear(BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(0)),
                        BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(1)), a);
                }
                if (cv < (_max - _fallOff))
                {
                    return BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(1));
                }
                if (cv < (_max + _fallOff))
                {
                    var lc = (_max - _fallOff);
                    var uc = (_max + _fallOff);
                    a = Utils.MapCubicSCurve((cv - lc) / (uc - lc));
                    return Utils.InterpolateLinear(BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(1)),
                        BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(0)), a);
                }
                return BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(0));
            }
            if (cv < _min || cv > _max)
            {
                return BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(0));
            }
            return BurstModuleManager.GetBurstValue(x, y, z, data, moduleData.Source(1));
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Select.
        /// </summary>
        public Select()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Select.
        /// </summary>
        /// <param name="inputA">The first input module.</param>
        /// <param name="inputB">The second input module.</param>
        /// <param name="controller">The controller module.</param>
        public Select(ModuleBase inputA, ModuleBase inputB, ModuleBase controller)
            : base(inputA, inputB, controller)
        {
        }

        /// <summary>
        /// Initializes a new instance of Select.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="fallOff">The falloff value at the edge transition.</param>
        /// <param name="inputA">The first input module.</param>
        /// <param name="inputB">The second input module.</param>
        public Select(double min, double max, double fallOff, ModuleBase inputA, ModuleBase inputB)
            : base(min, max, fallOff, inputA, inputB)
        {
        }

        #endregion
    }
}