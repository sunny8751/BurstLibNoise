using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Operator;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "Operator/Translate")]
	public class TranslateNode : BurstLibNoiseNode
	{
		public const string ID = "translateNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "Translate"; } }
		
		[ValueConnectionKnob("", Direction.In, "BurstModuleBase")]
		public ValueConnectionKnob inputModuleKnob;
		
		[ValueConnectionKnob("", Direction.Out, "BurstModuleBase")]
		public ValueConnectionKnob outputModuleKnob;

		public float x = 0f;
		public float y = 0f;
		public float z = 0f;

		public override void BurstLibNoiseNodeGUI()
		{
            inputModuleKnob.DisplayLayout();
			outputModuleKnob.DisplayLayout();

			DrawTexture();

			x = EditorGUILayout.FloatField("X", x);
			y = EditorGUILayout.FloatField("Y", y);
			z = EditorGUILayout.FloatField("Z", z);
		}

		public override bool Calculate()
		{
            BurstModuleBase inputModule = inputModuleKnob.connected() ? inputModuleKnob.GetValue<BurstModuleBase>() : null;
            if (inputModule != null) {
                Translate module = new Translate(x, y, z, inputModule);
                outputModuleKnob.SetValue(module);
    			tex = GenerateTex(module);
            } else {
                tex = defaultTex;
            }
			return true;
		}

	}
}