using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Generator;
using System;

namespace BurstLibNoise
{
    /// <summary>
    /// A plane terrain density calculation job
    /// </summary>
    [BurstCompile]
    struct GenerateLibNoiseJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<ModuleData> moduleData;

        [WriteOnly] public NativeArray<float> heightmap;

        [ReadOnly] public int width;

        [ReadOnly] public int height;

        [ReadOnly] public GenerateMode generateMode;

        [ReadOnly] public double p1;

        [ReadOnly] public double p2;

        [ReadOnly] public double p3;

        [ReadOnly] public double p4;

        [ReadOnly] public bool p5;

        /// <summary>
        /// The execute method required for Unity's IJobParallelFor job type
        /// </summary>
        /// <param name="index">The iteration index provided by Unity's Job System</param>
        public void Execute(int index)
        {
            int positionX = (index / (width * height));
            int positionY = (index / width % height);
            int positionZ = (index % width);

            heightmap[index] = Calculate(positionX, positionY, positionZ);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Calculate(int x, int y, int z)
        {
            if (generateMode == GenerateMode.Planar) {
                return CalculatePlanar(x, y, z);
            } else if (generateMode == GenerateMode.Cylindrical) {
                return CalculateCylindrical(x, y, z);
            } else if (generateMode == GenerateMode.Spherical) {
                return CalculateSpherical(x, y, z);
            } else {
                return BurstModuleManager.GetBurstValue(x, y, z, moduleData, 0);
            }
        }

        /// <summary>
        /// Generates a spherical projection of a point in the noise map.
        /// </summary>
        /// <param name="lat">The latitude of the point.</param>
        /// <param name="lon">The longitude of the point.</param>
        /// <returns>The corresponding noise map value.</returns>
        private double GenerateSpherical(double lat, double lon)
        {
            var r = Math.Cos(Mathf.Deg2Rad * lat);
            return BurstModuleManager.GetBurstValue((float) (r * Math.Cos(Mathf.Deg2Rad * lon)), (float) (Math.Sin(Mathf.Deg2Rad * lat)),
                (float) (r * Math.Sin(Mathf.Deg2Rad * lon)), moduleData, 0);
        }

        private float CalculateSpherical(int x, int y, int z) {
            int _ucBorder = 1;
            float _width = width - _ucBorder * 2;
            float _height = height - _ucBorder * 2;

            double south = p1;
            double north = p2;
            double west = p3;
            double east = p4;

            var loe = east - west;
            var lae = north - south;
            var xd = loe / ((double) _width - _ucBorder);
            var yd = lae / ((double) _height - _ucBorder);
            var clo = west + z * xd;
            var cla = south + y * yd;
            return (float) GenerateSpherical(cla, clo);
        }

        /// <summary>
        /// Generates a cylindrical projection of a point in the noise map.
        /// </summary>
        /// <param name="angle">The angle of the point.</param>
        /// <param name="height">The height of the point.</param>
        /// <returns>The corresponding noise map value.</returns>
        private double GenerateCylindrical(double angle, double height)
        {
            var x = math.cos(math.radians(angle));
            var y = height;
            var z = math.sin(math.radians(angle));
            return BurstModuleManager.GetBurstValue((float) x, (float) y, (float) z, moduleData, 0);
        }

        private float CalculateCylindrical(int x, int y, int z) {
            int _ucBorder = 1;
            float _width = width - _ucBorder * 2;
            float _height = height - _ucBorder * 2;

            double angleMin = p1;
            double angleMax = p2;
            double heightMin = p3;
            double heightMax = p4;

            var ae = angleMax - angleMin;
            var he = heightMax - heightMin;
            var xd = ae / ((double) _width - _ucBorder);
            var yd = he / ((double) _height - _ucBorder);
            var ca = angleMin + z * xd;
            var ch = heightMin + y * yd;
            return (float) GenerateCylindrical(ca, ch);
        }

        /// <summary>
        /// Generates a planar projection of a point in the noise map.
        /// </summary>
        /// <param name="x">The position on the x-axis.</param>
        /// <param name="y">The position on the y-axis.</param>
        /// <returns>The corresponding noise map value.</returns>
        private double GeneratePlanar(double x, double y)
        {
            return BurstModuleManager.GetBurstValue((float) x, 0.0f, (float) y, moduleData, 0);
        }

        private float CalculatePlanar(int x, int  y, int z) {
            int _ucBorder = 1;
            float _width = width - _ucBorder * 2;
            float _height = height - _ucBorder * 2;

            double left = p1;
            double right = p2;
            double top = p3;
            double bottom = p4;
            bool isSeamless = p5;

            var xe = right - left;
            var ze = bottom - top;
            var xd = xe / ((double) _width - _ucBorder);
            var zd = ze / ((double) _height - _ucBorder);
            var xc = left + z * xd;
            var zc = top + y * zd;

            float fv;
            if (isSeamless)
            {
                fv = (float) GeneratePlanar(xc, zc);
            }
            else
            {
                var swv = GeneratePlanar(xc, zc);
                var sev = GeneratePlanar(xc + xe, zc);
                var nwv = GeneratePlanar(xc, zc + ze);
                var nev = GeneratePlanar(xc + xe, zc + ze);
                var xb = 1.0 - ((xc - left) / xe);
                var zb = 1.0 - ((zc - top) / ze);
                var z0 = Utils.InterpolateLinear(swv, sev, xb);
                var z1 = Utils.InterpolateLinear(nwv, nev, xb);
                fv = (float) Utils.InterpolateLinear(z0, z1, zb);
            }
            return fv;
}
    }
}