using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Operator;
using NodeEditorFramework.Utilities;
using UnityEditor;
using Unity.Collections;
using UnityEngine.Windows;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "NoiseSettings/Input")]
	public class InputNode : BurstLibNoiseNode
	{
		public const string ID = "inputNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "Input"; } }
		public override Vector2 DefaultSize { get { return new Vector2(120, 120); } }

		[ValueConnectionKnob("", Direction.Out, "BurstModuleBase")]
		public ValueConnectionKnob outputModuleKnob;

		public string assetPath = "";
		
		public override void BurstLibNoiseNodeGUI()
		{
            outputModuleKnob.DisplayLayout();

			GUILayout.Label(System.IO.Path.GetFileNameWithoutExtension(assetPath));

            if (GUILayout.Button ("Open"))
            {
				LoadNoiseSettings();
            }

			DrawTexture();
		}

		private void LoadNoiseSettings() {
			string absolutePath = UnityEditor.EditorUtility.OpenFilePanel("Load NoiseSettings", NOISE_SETTINGS_SAVE_FOLDER, "asset");
			Debug.Assert(absolutePath.StartsWith(Application.dataPath));
			assetPath = "Assets" + absolutePath.Substring(Application.dataPath.Length);
		}

		public override bool Calculate()
		{
			if (string.IsNullOrEmpty(assetPath)) {
                tex = defaultTex;
				return true;
			}
			if (!File.Exists(assetPath)) {
                tex = defaultTex;
				return false;
			}
			BurstModuleBase module = ParseModuleData(((NoiseSettings) AssetDatabase.LoadAssetAtPath(assetPath, typeof(NoiseSettings))).moduleData);
			outputModuleKnob.SetValue(module);
			tex = GenerateTex(module);
			return true;
		}
	}
}