using System.Diagnostics;
using Unity.Collections;
using BurstLibNoise;

namespace BurstLibNoise.Operator
{
    /// <summary>
    /// Provides a noise module that outputs the value selected from one of two source
    /// modules chosen by the output value from a control module. [OPERATOR]
    /// </summary>
    public class Select
    {
        public static ModuleData GetData(float min=-1, float max=1, float fallOff=0) {
            return new ModuleData(ModuleType.Select, min, max, fallOff);
        }

        #region ModuleBase Members

        /// <summary>
        /// Returns the output value for the given input coordinates.
        /// </summary>
        /// <param name="x">The input coordinate on the x-axis.</param>
        /// <param name="y">The input coordinate on the y-axis.</param>
        /// <param name="z">The input coordinate on the z-axis.</param>
        /// <returns>The resulting output value.</returns>
        public static float GetValue(float x, float y, float z, NativeArray<ModuleData> data, int dataIndex)
        {
            // Debug.Assert(dataIndex + 3 < data.Length);
            ModuleData selectData = data[dataIndex];
            float _min = selectData[0];
            float _max = selectData[1];
            float _fallOff = selectData[2];
            float cv = ModuleBase.GetValue(x, y, z, data, dataIndex + 3);
            if (_fallOff > 0.0)
            {
                float a;
                if (cv < (_min - _fallOff))
                {
                    return ModuleBase.GetValue(x, y, z, data, dataIndex + 1);
                }
                if (cv < (_min + _fallOff))
                {
                    var lc = (_min - _fallOff);
                    var uc = (_min + _fallOff);
                    a = Utils.MapCubicSCurve((cv - lc) / (uc - lc));
                    return Utils.InterpolateLinear(ModuleBase.GetValue(x, y, z, data, dataIndex + 1),
                        ModuleBase.GetValue(x, y, z, data, dataIndex + 2), a);
                }
                if (cv < (_max - _fallOff))
                {
                    return ModuleBase.GetValue(x, y, z, data, dataIndex + 2);
                }
                if (cv < (_max + _fallOff))
                {
                    var lc = (_max - _fallOff);
                    var uc = (_max + _fallOff);
                    a = Utils.MapCubicSCurve((cv - lc) / (uc - lc));
                    return Utils.InterpolateLinear(ModuleBase.GetValue(x, y, z, data, dataIndex + 2),
                        ModuleBase.GetValue(x, y, z, data, dataIndex + 1), a);
                }
                return ModuleBase.GetValue(x, y, z, data, dataIndex + 1);
            }
            if (cv < _min || cv > _max)
            {
                return ModuleBase.GetValue(x, y, z, data, dataIndex + 1);
            }
            return ModuleBase.GetValue(x, y, z, data, dataIndex + 2);
        }

        #endregion
    }
}