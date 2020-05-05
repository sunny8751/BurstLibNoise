using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using BurstLibNoise;
using LibNoise;

namespace BurstLibNoise.Generator
{
    public class Voronoi : LibNoise.Generator.Voronoi, BurstModuleBase
    {
        public ModuleData GetData(int[] sources) {
            return new ModuleData(ModuleType.Voronoi, sources, (float) Frequency, (float) Displacement, Seed, UseDistance ? 1 : 0);
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
            float _displacement = moduleData[1];
            int _seed = (int) moduleData[2];
            bool _distance = ((int) moduleData[3]) == 1;
            
            x *= _frequency;
            y *= _frequency;
            z *= _frequency;
            var xi = (x > 0.0f ? (int) x : (int) x - 1);
            var iy = (y > 0.0f ? (int) y : (int) y - 1);
            var iz = (z > 0.0f ? (int) z : (int) z - 1);
            var md = 2147483647.0f;
            float xc = 0;
            float yc = 0;
            float zc = 0;
            for (var zcu = iz - 2; zcu <= iz + 2; zcu++)
            {
                for (var ycu = iy - 2; ycu <= iy + 2; ycu++)
                {
                    for (var xcu = xi - 2; xcu <= xi + 2; xcu++)
                    {
                        var xp = xcu + Utils.ValueNoise3D(xcu, ycu, zcu, _seed);
                        var yp = ycu + Utils.ValueNoise3D(xcu, ycu, zcu, _seed + 1);
                        var zp = zcu + Utils.ValueNoise3D(xcu, ycu, zcu, _seed + 2);
                        var xd = xp - x;
                        var yd = yp - y;
                        var zd = zp - z;
                        var d = xd * xd + yd * yd + zd * zd;
                        if (d < md)
                        {
                            md = d;
                            xc = xp;
                            yc = yp;
                            zc = zp;
                        }
                    }
                }
            }
            float v;
            if (_distance)
            {
                var xd = xc - x;
                var yd = yc - y;
                var zd = zc - z;
                v = (math.sqrt(xd * xd + yd * yd + zd * zd)) * Utils.Sqrt3 - 1.0f;
            }
            else
            {
                v = 0.0f;
            }
            return v + (_displacement * Utils.ValueNoise3D((int) (math.floor(xc)), (int) (math.floor(yc)),
                (int) (math.floor(zc)), 0));
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Voronoi.
        /// </summary>
        public Voronoi()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of Voronoi.
        /// </summary>
        /// <param name="frequency">The frequency of the first octave.</param>
        /// <param name="displacement">The displacement of the ridged-multifractal noise.</param>
        /// <param name="seed">The seed of the ridged-multifractal noise.</param>
        /// <param name="distance">Indicates whether the distance from the nearest seed point is applied to the output value.</param>
        public Voronoi(double frequency, double displacement, int seed, bool distance)
            : base(frequency, displacement, seed, distance)
        {
        }

        #endregion
    }
}