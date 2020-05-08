using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Generator;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "Generator/Cylinders")]
	public class CylindersNode : BurstLibNoiseNode
	{
		public const string ID = "cylindersNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "Cylinders"; } }

		[ValueConnectionKnob("", Direction.Out, "BurstModuleBase")]
		public ValueConnectionKnob outputModuleKnob;

		public float frequency = 1.0f;

		public override void BurstLibNoiseNodeGUI()
		{
			outputModuleKnob.DisplayLayout();

			DrawTexture();

			frequency = EditorGUILayout.FloatField("Frequency", frequency);
		}

		public override bool Calculate()
		{
            Cylinders module = new Cylinders(frequency);
			outputModuleKnob.SetValue(module);
			tex = GenerateTex(module);
			return true;
		}

	}
}