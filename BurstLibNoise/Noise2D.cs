using System;
using System.Xml.Serialization;
using UnityEngine;
using BurstLibNoise.Generator;
using LibNoise;
using Unity.Collections;

namespace BurstLibNoise
{
    /// <summary>
    /// Provides a two-dimensional noise map.
    /// </summary>
	/// <remarks>This covers most of the functionality from LibNoise's noiseutils library, but 
	/// the method calls might not be the same. See the tutorials project if you're wondering
	/// which calls are equivalent.</remarks>
    public class Noise2D : LibNoise.Noise2D
    {

        #region Fields

        private BurstModuleBase generator;
        private NativeArray<float> heightmap;
        
        private readonly int heightmapWidth;
        private readonly int heightmapHeight;
        private int _ucBorder = 1; // Border size of extra noise for uncropped data.

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Noise2D.
        /// </summary>
        protected Noise2D()
        {
        }

        /// <summary>
        /// Initializes a new instance of Noise2D.
        /// </summary>
        /// <param name="size">The width and height of the noise map.</param>
        public Noise2D(int size)
            : base(size, size, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of Noise2D.
        /// </summary>
        /// <param name="size">The width and height of the noise map.</param>
        /// <param name="generator">The generator module.</param>
        public Noise2D(int size, BurstModuleBase generator)
            : this(size, size, generator)
        {
        }

        /// <summary>
        /// Initializes a new instance of Noise2D.
        /// </summary>
        /// <param name="width">The width of the noise map.</param>
        /// <param name="height">The height of the noise map.</param>
        /// <param name="generator">The generator module.</param>
        public Noise2D(int width, int height, BurstModuleBase generator = null)
            : base(width, height, new Perlin())
        {
            this.generator = generator;
            heightmapWidth = width + _ucBorder * 2;
            heightmapHeight = height + _ucBorder * 2;
            this.heightmap = new NativeArray<float>(heightmapWidth * heightmapHeight, Allocator.Persistent);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the noise map.
        /// </summary>
        /// <param name="value">The constant value to clear the noise map with.</param>
        public new void Clear(float value = 0f)
        {
            base.Clear(value);
            for (var i = 0; i < heightmap.Length; i++)
            {
                heightmap[i] = value;
            }
        }

        /// <summary>
        /// Generates a non-seamless planar projection of the noise map.
        /// </summary>
        /// <param name="left">The clip region to the left.</param>
        /// <param name="right">The clip region to the right.</param>
        /// <param name="top">The clip region to the top.</param>
        /// <param name="bottom">The clip region to the bottom.</param>
        /// <param name="isSeamless">Indicates whether the resulting noise map should be seamless.</param>
        public new void GeneratePlanar(double left, double right, double top, double bottom, bool isSeamless = true)
        {
            if (right <= left || bottom <= top)
            {
                throw new ArgumentException("Invalid right/left or bottom/top combination");
            }
            if (generator == null)
            {
                throw new ArgumentNullException("Generator is null");
            }
            BurstModuleManager.GeneratePlanarHeightmap(heightmap, generator, heightmapWidth, heightmapHeight, left, right, top, bottom, isSeamless);
            SetData();
        }

        /// <summary>
        /// Generates a cylindrical projection of the noise map.
        /// </summary>
        /// <param name="angleMin">The maximum angle of the clip region.</param>
        /// <param name="angleMax">The minimum angle of the clip region.</param>
        /// <param name="heightMin">The minimum height of the clip region.</param>
        /// <param name="heightMax">The maximum height of the clip region.</param>
        public new void GenerateCylindrical(double angleMin, double angleMax, double heightMin, double heightMax)
        {
            if (angleMax <= angleMin || heightMax <= heightMin)
            {
                throw new ArgumentException("Invalid angle or height parameters");
            }
            if (generator == null)
            {
                throw new ArgumentNullException("Generator is null");
            }
            BurstModuleManager.GenerateCylindricalHeightmap(heightmap, generator, heightmapWidth, heightmapHeight, angleMin, angleMax, heightMin, heightMax);
            SetData();
        }

        /// <summary>
        /// Generates a spherical projection of the noise map.
        /// </summary>
        /// <param name="south">The clip region to the south.</param>
        /// <param name="north">The clip region to the north.</param>
        /// <param name="west">The clip region to the west.</param>
        /// <param name="east">The clip region to the east.</param>
        public new void GenerateSpherical(double south, double north, double west, double east)
        {
            if (east <= west || south <= north)
            {
                throw new ArgumentException("Invalid east/west or north/south combination");
            }
            if (generator == null)
            {
                throw new ArgumentNullException("Generator is null");
            }
            BurstModuleManager.GenerateSphericalHeightmap(heightmap, generator, heightmapWidth, heightmapHeight, south, north, west, east);
            SetData();            
        }

        private void SetData() {
            for (var x = 0; x < heightmapWidth; x++)
            {
                for (var y = 0; y < heightmapHeight; y++)
                {
                    this[x, y, false] = heightmap[x + heightmapWidth * y];
                    if (x >= _ucBorder && y >= _ucBorder && x < Width + _ucBorder &&
                        y < Height + _ucBorder)
                    {
                        this[x - _ucBorder, y - _ucBorder, true] = this[x, y, false]; // Cropped data
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Immediately releases the unmanaged resources used by this object.
        /// </summary>
        public new void Dispose()
        {
            heightmap.Dispose();
        }
    }
}
