using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Operator;
using NodeEditorFramework.Utilities;
using UnityEditor;
using Unity.Collections;
using UnityEngine.Windows;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "NoiseSettings/Output")]
	public class OutputNode : BurstLibNoiseNode
	{

		public const string ID = "outputNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "Output"; } }
		public override Vector2 DefaultSize { get { return new Vector2(120, 120); } }
		
		[ValueConnectionKnob("", Direction.In, "BurstModuleBase")]
		public ValueConnectionKnob inputModuleKnob;

		public string assetPath;
		
		public override void BurstLibNoiseNodeGUI()
		{
            inputModuleKnob.DisplayLayout();

			GUILayout.Label(System.IO.Path.GetFileNameWithoutExtension(assetPath));

			if (inputModuleKnob.connected() && GUILayout.Button("Save")) {
				SaveModule();
			}
		}

		private void SaveModule() {
			if (string.IsNullOrEmpty(assetPath)) {
				string absolutePath = UnityEditor.EditorUtility.SaveFilePanel("Save NoiseSettings", NOISE_SETTINGS_SAVE_FOLDER, "noiseSettings", "asset");
				Debug.Assert(absolutePath.StartsWith(Application.dataPath));
				assetPath = "Assets" + absolutePath.Substring(Application.dataPath.Length);
			}
			Debug.Assert(!string.IsNullOrEmpty(assetPath));

			NoiseSettings noiseSettings;
			if (File.Exists(assetPath)) {
				// overwrite existing
				noiseSettings = (NoiseSettings) AssetDatabase.LoadAssetAtPath(assetPath, typeof(NoiseSettings));
			} else {
				// save as new
				noiseSettings = (NoiseSettings) ScriptableObject.CreateInstance("NoiseSettings");
				AssetDatabase.CreateAsset (noiseSettings, assetPath);
			}
			
			noiseSettings.moduleData = GetModuleData(inputModuleKnob.GetValue<BurstModuleBase>());
			
			AssetDatabase.SaveAssets();
			EditorUtility.SetDirty(noiseSettings);
		}
	}
}