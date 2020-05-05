using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using BurstLibNoise;
using BurstLibNoise.Generator;
using BurstLibNoise.Operator;
using LibNoise;

public class BurstJobTest : MonoBehaviour
{
	[SerializeField] int Width = 0;

	[SerializeField] int Height = 0;

    private void Start() {
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
        Run(finalTerrain);
        Debug.Log(Time.realtimeSinceStartup - start);
    }

    private List<ModuleData> GetModuleData(BurstModuleBase root) {
        List<ModuleData> modules = new List<ModuleData>();
        Queue<BurstModuleBase> queue = new Queue<BurstModuleBase>();
        queue.Enqueue(root);

        while (queue.Count > 0) {
            BurstModuleBase module = queue.Dequeue();
            int[] sourceIndices = new int[((ModuleBase) module).SourceModuleCount];
            for (int i = 0; i < sourceIndices.Length; i++) {
                queue.Enqueue(module.Source(i));
                sourceIndices[i] = modules.Count + i + 1; // add one for the current module
            }
            modules.Add(module.GetData(sourceIndices));
            Debug.Log(module.GetData(sourceIndices).type);
        }
        return modules;
    }

    public void Run(BurstModuleBase module) {
        List<ModuleData> modules = GetModuleData(module);

        // TODO replace with unsafe mem copy
        NativeArray<ModuleData> moduleData = new NativeArray<ModuleData>(modules.Count, Allocator.Persistent);
        for (int i = 0; i < modules.Count; i++) {
            moduleData[i] = modules[i];
        }

        float scale = 1f / Width;
        
        NativeArray<float> heightmap = new NativeArray<float>(Width * Height, Allocator.Persistent);
        var job = new BurstJob
        {
            moduleData = moduleData,
            heightmap = heightmap,
            width = Width,
            height = Height,
            scale = scale,
        };
        JobHandle jobHandle = job.Schedule(heightmap.Length, 256);

        jobHandle.Complete();

        // create texture
        Texture2D texture = new Texture2D(Width, Height);
        float[] heightmapValues = heightmap.ToArray();
        Color[] colors = new Color[heightmapValues.Length];
        for (int i = 0; i < heightmapValues.Length; i++) {
            float value = heightmapValues[i];
            value = (value + 1) / 2;
            colors[i] = new Color(value, value, value, 1);
        }
        // Debug.Log(colors[0]);
        // Debug.Log(colors[5]);
        texture.SetPixels(colors);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

		GetComponent<Renderer>().material.mainTexture = texture;

        moduleData.Dispose();
        heightmap.Dispose();
    }

    private int Seed(System.Random random) {
        return random.Next();
    }
}
