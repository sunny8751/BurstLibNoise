using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
// using BurstLibNoise;
// using BurstLibNoise.Generator;
// using BurstLibNoise.Operator;
using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;
using BurstLibNoise.Manager;
using Unity.Mathematics;

public class BurstJobTest : MonoBehaviour
{
	[SerializeField] int Width = 0;

	[SerializeField] int Height = 0;

    private void Start() {
        // int seed = 0;
        // System.Random random = new System.Random(seed);

        // List<ModuleData> modules = new List<ModuleData>();
        // // modules.Add(Perlin.GetData(seed: Seed(random)));
        // modules.Add(RidgedMultifractal.GetData(seed: Seed(random)));

		// // var mountainTerrain = new RidgedMultifractal();
		// // var baseFlatTerrain = new Billow.GetData(frequency: 2.0f);
		// // var flatTerrain = new ScaleBias.GetData(0.125, -0.75, baseFlatTerrain);
		// // var terrainType = new Perlin.GetData(frequency: 0.5f, persistence: 0.25f);
		// // var finalTerrain = new Select.GetData(min: 0, max: 1000, fallOff: 0.125f);//(flatTerrain, mountainTerrain, terrainType);

        var terrain = new Perlin();
        Run(terrain);
    }

    private List<ModuleData> GetModuleData(ModuleBase root) {
        List<ModuleData> modules = new List<ModuleData>();
        Queue<ModuleBase> queue = new Queue<ModuleBase>();
        queue.Enqueue(root);

        while (queue.Count > 0) {
            ModuleBase module = queue.Dequeue();
            int[] sourceIndices = new int[module.SourceModuleCount];
            for (int i = 0; i < sourceIndices.Length; i++) {
                queue.Enqueue(module.Modules[i]);
                sourceIndices[i] = modules.Count + i + 1; // add one for the current module
            }
            modules.Add(module.GetData(sourceIndices));
        }
        return modules;
    }

    public void Run(ModuleBase module) {
        List<ModuleData> modules = GetModuleData(module);

        foreach (ModuleData data in modules) {
            Debug.Log(data.type);
        }

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
