using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Generator;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "Generator/Voronoi")]
	public class VoronoiNode : BurstLibNoiseNode
	{
		public const string ID = "voronoiNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "Voronoi"; } }

		[ValueConnectionKnob("", Direction.Out, "BurstModuleBase")]
		public ValueConnectionKnob outputModuleKnob;

		public float frequency = 1.0f;
		public float displacement = 2.0f;
		public int seed;
		public bool useDistance;

		public override void BurstLibNoiseNodeGUI()
		{
			outputModuleKnob.DisplayLayout();

			DrawTexture();

			frequency = EditorGUILayout.FloatField("Frequency", frequency);
			displacement = EditorGUILayout.FloatField("Lacunarity", displacement);
			useDistance = EditorGUILayout.Toggle(useDistance);
		}

		public override bool Calculate()
		{
            Voronoi module = new Voronoi(frequency, displacement, seed, useDistance);
			outputModuleKnob.SetValue(module);
			tex = GenerateTex(module);
			return true;
		}

	}
}