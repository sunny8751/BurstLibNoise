using System.Diagnostics;
using Unity.Collections;
using BurstLibNoise.Manager;

namespace BurstLibNoise.Operator
{
    /// <summary>
    /// Provides a noise module that outputs the value selected from one of two source
    /// modules chosen by the output value from a control module. [OPERATOR]
    /// </summary>
    public static class Select
    {
        public static ModuleData GetData(this LibNoise.Operator.Select select, int[] sources) {
            return new ModuleData(ModuleType.Select, sources, (float) select.Minimum, (float) select.Maximum, (float) select.FallOff);
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
            ModuleData selectData = data[dataIndex];
            float _min = selectData[0];
            float _max = selectData[1];
            float _fallOff = selectData[2];
            float cv = BurstModuleManager.GetBurstValue(x, y, z, data, selectData.Source(2));
            if (_fallOff > 0.0)
            {
                float a;
                if (cv < (_min - _fallOff))
                {
                    return BurstModuleManager.GetBurstValue(x, y, z, data, selectData.Source(0));
                }
                if (cv < (_min + _fallOff))
                {
                    var lc = (_min - _fallOff);
                    var uc = (_min + _fallOff);
                    a = Utils.MapCubicSCurve((cv - lc) / (uc - lc));
                    return Utils.InterpolateLinear(BurstModuleManager.GetBurstValue(x, y, z, data, dataIndex + 1),
                        BurstModuleManager.GetBurstValue(x, y, z, data, selectData.Source(1)), a);
                }
                if (cv < (_max - _fallOff))
                {
                    return BurstModuleManager.GetBurstValue(x, y, z, data, selectData.Source(1));
                }
                if (cv < (_max + _fallOff))
                {
                    var lc = (_max - _fallOff);
                    var uc = (_max + _fallOff);
                    a = Utils.MapCubicSCurve((cv - lc) / (uc - lc));
                    return Utils.InterpolateLinear(BurstModuleManager.GetBurstValue(x, y, z, data, dataIndex + 2),
                        BurstModuleManager.GetBurstValue(x, y, z, data, selectData.Source(0)), a);
                }
                return BurstModuleManager.GetBurstValue(x, y, z, data, selectData.Source(0));
            }
            if (cv < _min || cv > _max)
            {
                return BurstModuleManager.GetBurstValue(x, y, z, data, selectData.Source(0));
            }
            return BurstModuleManager.GetBurstValue(x, y, z, data, selectData.Source(1));
        }
    }
}