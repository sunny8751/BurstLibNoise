using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Operator;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "Operator/Displace")]
	public class DisplaceNode : BurstLibNoiseNode
	{
		public const string ID = "displaceNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "Displace"; } }
		
		[ValueConnectionKnob("", Direction.In, "BurstModuleBase")]
		public ValueConnectionKnob inputModule1Knob;
		[ValueConnectionKnob("", Direction.In, "BurstModuleBase")]
		public ValueConnectionKnob inputModule2Knob;
		[ValueConnectionKnob("", Direction.In, "BurstModuleBase")]
		public ValueConnectionKnob inputModule3Knob;
		[ValueConnectionKnob("", Direction.In, "BurstModuleBase")]
		public ValueConnectionKnob inputModule4Knob;
		
		[ValueConnectionKnob("", Direction.Out, "BurstModuleBase")]
		public ValueConnectionKnob outputModuleKnob;

		public override void BurstLibNoiseNodeGUI()
		{
            inputModule1Knob.DisplayLayout();
            inputModule2Knob.DisplayLayout();
            inputModule3Knob.DisplayLayout();
            inputModule4Knob.DisplayLayout();
			outputModuleKnob.DisplayLayout();

			DrawTexture();
		}

		public override bool Calculate()
		{
            BurstModuleBase inputModule1 = inputModule1Knob.connected() ? inputModule1Knob.GetValue<BurstModuleBase>() : null;
            BurstModuleBase inputModule2 = inputModule2Knob.connected() ? inputModule2Knob.GetValue<BurstModuleBase>() : null;
            BurstModuleBase inputModule3 = inputModule3Knob.connected() ? inputModule3Knob.GetValue<BurstModuleBase>() : null;
            BurstModuleBase inputModule4 = inputModule4Knob.connected() ? inputModule4Knob.GetValue<BurstModuleBase>() : null;
            if (inputModule1 != null && inputModule2 != null && inputModule3 != null && inputModule4 != null) {
                Displace module = new Displace(inputModule1, inputModule2, inputModule3, inputModule4);
                outputModuleKnob.SetValue(module);
    			tex = GenerateTex(module);
            } else {
                tex = defaultTex;
            }
			return true;
		}

	}
}