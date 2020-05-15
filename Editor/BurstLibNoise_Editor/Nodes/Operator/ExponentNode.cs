using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Operator;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "Operator/Exponent")]
	public class ExponentNode : BurstLibNoiseNode
	{
		public const string ID = "exponentNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "Exponent"; } }
		
		[ValueConnectionKnob("", Direction.In, "BurstModuleBase")]
		public ValueConnectionKnob inputModuleKnob;
		
		[ValueConnectionKnob("", Direction.Out, "BurstModuleBase")]
		public ValueConnectionKnob outputModuleKnob;

		public float exponent = 1.0f;

		public override void BurstLibNoiseNodeGUI()
		{
            inputModuleKnob.DisplayLayout();
			outputModuleKnob.DisplayLayout();

			DrawTexture();

			exponent = EditorGUILayout.FloatField("Exponent", exponent);
		}

		public override bool Calculate()
		{
            BurstModuleBase inputModule = inputModuleKnob.connected() ? inputModuleKnob.GetValue<BurstModuleBase>() : null;
            if (inputModule != null) {
                Exponent module = new Exponent(exponent, inputModule);
                outputModuleKnob.SetValue(module);
    			tex = GenerateTex(module);
            } else {
                tex = defaultTex;
            }
			return true;
		}

	}
}