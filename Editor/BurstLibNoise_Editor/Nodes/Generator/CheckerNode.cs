using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Generator;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "Generator/Checker")]
	public class CheckerNode : BurstLibNoiseNode
	{
		public const string ID = "checkerNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "Checker"; } }

		[ValueConnectionKnob("", Direction.Out, "BurstModuleBase")]
		public ValueConnectionKnob outputModuleKnob;

		public override void BurstLibNoiseNodeGUI()
		{
			outputModuleKnob.DisplayLayout();

			DrawTexture();
		}

		public override bool Calculate()
		{
            Checker module = new Checker();
			outputModuleKnob.SetValue(module);
			tex = GenerateTex(module);
			return true;
		}

	}
}