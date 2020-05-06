using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise.Generator;
using LibNoise.Operator;
using LibNoise;

public class LibNoiseTest : MonoBehaviour
{
	[SerializeField] int Width = 0;

	[SerializeField] int Height = 0;

    // Start is called before the first frame update
    void Start()
    {
		// STEP 1
		// Gradient is set directly on the object
		var mountainTerrain = new RidgedMultifractal();
		// // STEP 2
		var baseFlatTerrain = new Billow();
		baseFlatTerrain.Frequency = 2.0;
		// STEP 3
		var flatTerrain = new ScaleBias(0.125, -0.75, baseFlatTerrain);
		// STEP 4
		var terrainType = new Perlin();
		terrainType.Frequency = 0.5;
		terrainType.Persistence = 0.25;

		var finalTerrain = new Select(flatTerrain, mountainTerrain, terrainType);
		finalTerrain.SetBounds(0, 1000);
		finalTerrain.FallOff = 0.125;

        float start = Time.realtimeSinceStartup;
        RenderAndSetImage(finalTerrain);
        Debug.Log("LibNoise runtime: " + (Time.realtimeSinceStartup - start));
    }

	void RenderAndSetImage(ModuleBase generator)
	{
		var heightMapBuilder = new Noise2D(Width, Height, generator);
        heightMapBuilder.GeneratePlanar(Noise2D.Left, Noise2D.Right, Noise2D.Top, Noise2D.Bottom);
		// heightMapBuilder.GenerateSpherical(90, -90, -180, 180);
        // heightMapBuilder.GenerateCylindrical(-180, 180, -1, 1);
		var image = heightMapBuilder.GetTexture();
		GetComponent<Renderer>().material.mainTexture = image;
	}
}
