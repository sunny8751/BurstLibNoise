using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Operator;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "Operator/Turbulence")]
	public class TurbulenceNode : BurstLibNoiseNode
	{
		public const string ID = "turbulenceNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "Turbulence"; } }
		
		[ValueConnectionKnob("", Direction.In, "BurstModuleBase")]
		public ValueConnectionKnob inputModuleKnob;
		
		[ValueConnectionKnob("", Direction.Out, "BurstModuleBase")]
		public ValueConnectionKnob outputModuleKnob;

		public float power = 1.0f;
		public float frequency = 1.0f;
		public int roughness = 6;
		public int seed;

		public override void BurstLibNoiseNodeGUI()
		{
            inputModuleKnob.DisplayLayout();
			outputModuleKnob.DisplayLayout();

			DrawTexture();

			power = EditorGUILayout.FloatField("Power", power);
			frequency = EditorGUILayout.FloatField("Frequency", frequency);
			roughness = EditorGUILayout.IntField("Roughness", roughness);
		}

		public override bool Calculate()
		{
            BurstModuleBase inputModule = inputModuleKnob.connected() ? inputModuleKnob.GetValue<BurstModuleBase>() : null;
            if (inputModule != null) {
                Turbulence module = new Turbulence(power, frequency, roughness, seed, inputModule);
                outputModuleKnob.SetValue(module);
    			tex = GenerateTex(module);
            } else {
                tex = defaultTex;
            }
			return true;
		}

	}
}