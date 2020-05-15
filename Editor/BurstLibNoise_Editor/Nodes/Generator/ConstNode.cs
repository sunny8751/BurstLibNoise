using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Generator;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "Generator/Const")]
	public class ConstNode : BurstLibNoiseNode
	{
		public const string ID = "constNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "Const"; } }

		[ValueConnectionKnob("", Direction.Out, "BurstModuleBase")]
		public ValueConnectionKnob outputModuleKnob;

		public float value = 0f;

		public override void BurstLibNoiseNodeGUI()
		{
			outputModuleKnob.DisplayLayout();

			DrawTexture();

			value = EditorGUILayout.FloatField("Value", value);
		}

		public override bool Calculate()
		{
            Const module = new Const(value);
			outputModuleKnob.SetValue(module);
			tex = GenerateTex(module);
			return true;
		}

	}
}