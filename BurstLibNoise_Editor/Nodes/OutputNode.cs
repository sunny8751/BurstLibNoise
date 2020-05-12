using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Operator;
using NodeEditorFramework.Utilities;
using UnityEditor;
using Unity.Collections;
using UnityEngine.Windows;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "Output")]
	public class OutputNode : BurstLibNoiseNode
	{
		private const string NOISE_SETTINGS_SAVE_FOLDER = "Assets/BurstLibNoise/BurstLibNoise/NoiseSettings/";

		public const string ID = "outputNode";
		public override string GetID { get { return ID; } }

		public override string Title { get { return "Output"; } }
		public override Vector2 DefaultSize { get { return new Vector2(120, 120); } }
		
		[ValueConnectionKnob("", Direction.In, "BurstModuleBase")]
		public ValueConnectionKnob inputModuleKnob;

		public string noiseName;
		
		public override void BurstLibNoiseNodeGUI()
		{
			backgroundColor = CanCalculate() ? Color.grey : Color.red;
            inputModuleKnob.DisplayLayout();

			noiseName = EditorGUILayout.TextField(noiseName);

			if (GUILayout.Button("Save")) {
				if (CanCalculate()) {
					BurstModuleBase inputModule = inputModuleKnob.GetValue<BurstModuleBase>();
					SaveModule(inputModule, noiseName);
				}
			}
		}

		// public override bool Calculate()
		// {
        //     if (CanCalculate()) {
	    //         BurstModuleBase inputModule = inputModuleKnob.GetValue<BurstModuleBase>();
		// 		SaveModule(inputModule, noiseName);
        //     }
		// 	return true;
		// }

		private bool CanCalculate() {
			return inputModuleKnob.connected() && !string.IsNullOrEmpty(noiseName);
		}

		private void SaveModule(BurstModuleBase moduleBase, string name) {
			string assetPath = NOISE_SETTINGS_SAVE_FOLDER + name.Replace(" ", "") + ".asset";

			NoiseSettings noiseSettings;
			if (File.Exists(assetPath)) {
				noiseSettings = (NoiseSettings) AssetDatabase.LoadAssetAtPath(assetPath, typeof(NoiseSettings));
			} else {
				noiseSettings = (NoiseSettings) ScriptableObject.CreateInstance("NoiseSettings");
				AssetDatabase.CreateAsset (noiseSettings, assetPath);
			}
			
			NativeArray<ModuleData> data = BurstModuleManager.CreateModuleData(moduleBase);
			noiseSettings.moduleData = data.ToArray();
			data.Dispose();
			
			AssetDatabase.SaveAssets ();
			EditorUtility.SetDirty(noiseSettings);
		}
	}
}