using System.Diagnostics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;
using Unity.Mathematics;

namespace BurstLibNoise.Operator
{
    public class Rotate : LibNoise.Operator.Rotate, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Rotate, sources, (float) X, (float) Y, (float) Z);
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
            
            // need to recalculate the matrix every time because we can't access them
            var xc = math.cos(math.radians(_x));
            var yc = math.cos(math.radians(_y));
            var zc = math.cos(math.radians(_z));
            var xs = math.sin(math.radians(_x));
            var ys = math.sin(math.radians(_y));
            var zs = math.sin(math.radians(_z));
            var _x1Matrix = ys * xs * zs + yc * zc;
            var _y1Matrix = xc * zs;
            var _z1Matrix = ys * zc - yc * xs * zs;
            var _x2Matrix = ys * xs * zc - yc * zs;
            var _y2Matrix = xc * zc;
            var _z2Matrix = -yc * xs * zc - ys * zs;
            var _x3Matrix = -ys * xc;
            var _y3Matrix = xs;
            var _z3Matrix = yc * xc;
            
            // Debug.Assert(Modules[0] != null);
            var nx = (_x1Matrix * x) + (_y1Matrix * y) + (_z1Matrix * z);
            var ny = (_x2Matrix * x) + (_y2Matrix * y) + (_z2Matrix * z);
            var nz = (_x3Matrix * x) + (_y3Matrix * y) + (_z3Matrix * z);
            return BurstModuleManager.GetBurstValue(nx, ny, nz, data, moduleData.Source(0));
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Rotate.
        /// </summary>
        public Rotate()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Rotate.
        /// </summary>
        /// <param name="input">The input module.</param>
        public Rotate(ModuleBase input)
            : base(input)
        {
        }

        /// <summary>
        /// Initializes a new instance of Rotate.
        /// </summary>
        /// <param name="x">The rotation around the x-axis.</param>
        /// <param name="y">The rotation around the y-axis.</param>
        /// <param name="z">The rotation around the z-axis.</param>
        /// <param name="input">The input module.</param>
        public Rotate(double x, double y, double z, ModuleBase input)
            : base(x, y, z, input)
        {
        }

        #endregion
    }
}