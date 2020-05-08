using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Operator;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "Operator/Min")]
	public class MinNode : BurstLibNoiseNode
	{
		public const string ID = "minNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "Min"; } }
		
		[ValueConnectionKnob("", Direction.In, "BurstModuleBase")]
		public ValueConnectionKnob inputModule1Knob;
		[ValueConnectionKnob("", Direction.In, "BurstModuleBase")]
		public ValueConnectionKnob inputModule2Knob;
		
		[ValueConnectionKnob("", Direction.Out, "BurstModuleBase")]
		public ValueConnectionKnob outputModuleKnob;

		public override void BurstLibNoiseNodeGUI()
		{
            inputModule1Knob.DisplayLayout();
            inputModule2Knob.DisplayLayout();
			outputModuleKnob.DisplayLayout();

			DrawTexture();
		}

		public override bool Calculate()
		{
            BurstModuleBase inputModule1 = inputModule1Knob.connected() ? inputModule1Knob.GetValue<BurstModuleBase>() : null;
            BurstModuleBase inputModule2 = inputModule2Knob.connected() ? inputModule2Knob.GetValue<BurstModuleBase>() : null;
            if (inputModule1 != null && inputModule2 != null) {
                Min module = new Min(inputModule1, inputModule2);
                outputModuleKnob.SetValue(module);
    			tex = GenerateTex(module);
            } else {
                tex = defaultTex;
            }
			return true;
		}

	}
}