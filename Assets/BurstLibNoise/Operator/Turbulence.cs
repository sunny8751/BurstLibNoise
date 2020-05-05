using System.Diagnostics;
using Unity.Collections;
using BurstLibNoise;
using BurstLibNoise.Generator;
using LibNoise;

namespace BurstLibNoise.Operator
{
    public class Turbulence : LibNoise.Operator.Turbulence, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            float _lacunarity = (float) new Perlin().Lacunarity;
            float _persistence = (float) new Perlin().Persistence;
            return new ModuleData(ModuleType.Turbulence, sources, (float) Power, (float) Frequency, _lacunarity, _persistence, (int) Roughness, (int) Seed);
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
            float _power = moduleData[0];
            float _frequency = moduleData[1];
            float _lacunarity = moduleData[2];
            float _persistence = moduleData[3];
            int _octaveCount = (int) moduleData[4];
            int _seed = (int) moduleData[5];
            
            // Debug.Assert(Modules[0] != null);
            var xd = x + (Perlin.GetBurstValue(x + X0, y + Y0, z + Z0, _frequency, _lacunarity, _persistence, _octaveCount, _seed) * _power);
            var yd = y + (Perlin.GetBurstValue(x + X1, y + Y1, z + Z1, _frequency, _lacunarity, _persistence, _octaveCount, _seed) * _power);
            var zd = z + (Perlin.GetBurstValue(x + X2, y + Y2, z + Z2, _frequency, _lacunarity, _persistence, _octaveCount, _seed) * _power);
            return BurstModuleManager.GetBurstValue(xd, yd, zd, data, moduleData.Source(0));
        }
        
        #region Constants

        private const float X0 = (12414.0f / 65536.0f);
        private const float Y0 = (65124.0f / 65536.0f);
        private const float Z0 = (31337.0f / 65536.0f);
        private const float X1 = (26519.0f / 65536.0f);
        private const float Y1 = (18128.0f / 65536.0f);
        private const float Z1 = (60493.0f / 65536.0f);
        private const float X2 = (53820.0f / 65536.0f);
        private const float Y2 = (11213.0f / 65536.0f);
        private const float Z2 = (44845.0f / 65536.0f);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Turbulence.
        /// </summary>
        public Turbulence()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Turbulence.
        /// </summary>
        /// <param name="input">The input module.</param>
        public Turbulence(ModuleBase input)
            : base(input)
        {

        }

        /// <summary>
        /// Initializes a new instance of Turbulence.
        /// </summary>
        public Turbulence(double power, ModuleBase input)
            : this(new Perlin(), new Perlin(), new Perlin(), power, input)
        {
        }

        /// <summary>
        /// Initializes a new instance of Turbulence.
        /// </summary>
        /// <param name="x">The perlin noise to apply on the x-axis.</param>
        /// <param name="y">The perlin noise to apply on the y-axis.</param>
        /// <param name="z">The perlin noise to apply on the z-axis.</param>
        /// <param name="power">The power of the turbulence.</param>
        /// <param name="input">The input module.</param>
        public Turbulence(Perlin x, Perlin y, Perlin z, double power, ModuleBase input)
            : base(x, y, z, power, input)
        {
        }

        #endregion
    }
}