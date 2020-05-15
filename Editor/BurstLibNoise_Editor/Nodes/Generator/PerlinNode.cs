using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Generator;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "Generator/Perlin")]
	public class PerlinNode : BurstLibNoiseNode
	{
		public const string ID = "perlinNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "Perlin"; } }

		[ValueConnectionKnob("", Direction.Out, "BurstModuleBase")]
		public ValueConnectionKnob outputModuleKnob;

		public float frequency = 1.0f;
		public float lacunarity = 2.0f;
		public float persistence = 0.5f;
		public int octaveCount = 6;
		public int seed;

		public override void BurstLibNoiseNodeGUI()
		{
			outputModuleKnob.DisplayLayout();

			DrawTexture();

			frequency = EditorGUILayout.FloatField("Frequency", frequency);
			lacunarity = EditorGUILayout.FloatField("Lacunarity", lacunarity);
			persistence = EditorGUILayout.FloatField("Persistence", persistence);
			octaveCount = EditorGUILayout.IntField("Octaves", octaveCount);
		}

		public override bool Calculate()
		{
            Perlin module = new Perlin(frequency, lacunarity, persistence, octaveCount, seed, LibNoise.QualityMode.Medium);
			outputModuleKnob.SetValue(module);
			tex = GenerateTex(module);
			return true;
		}

	}
}