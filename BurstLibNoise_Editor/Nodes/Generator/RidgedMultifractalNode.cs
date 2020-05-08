using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Generator;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "Generator/RidgedMultifractal")]
	public class RidgedMultifractalNode : BurstLibNoiseNode
	{
		public const string ID = "ridgedMultifractalNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "RidgedMultifractal"; } }

		[ValueConnectionKnob("", Direction.Out, "BurstModuleBase")]
		public ValueConnectionKnob outputModuleKnob;

		public float frequency = 1.0f;
		public float lacunarity = 2.0f;
		public int octaveCount = 6;
		public int seed;

		public override void BurstLibNoiseNodeGUI()
		{
			outputModuleKnob.DisplayLayout();

			DrawTexture();

			frequency = EditorGUILayout.FloatField("Frequency", frequency);
			lacunarity = EditorGUILayout.FloatField("Lacunarity", lacunarity);
			octaveCount = EditorGUILayout.IntField("Octaves", octaveCount);
		}

		public override bool Calculate()
		{
            RidgedMultifractal module = new RidgedMultifractal(frequency, lacunarity, octaveCount, seed, LibNoise.QualityMode.Medium);
			outputModuleKnob.SetValue(module);
			tex = GenerateTex(module);
			return true;
		}

	}
}