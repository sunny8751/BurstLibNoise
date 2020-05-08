using UnityEngine;
using BurstLibNoise;
using BurstLibNoise.Operator;
using NodeEditorFramework.Utilities;
using UnityEditor;
using Unity.Collections;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	[Node(false, "Output")]
	public class OutputNode : BurstLibNoiseNode
	{
		private const string NOISE_SETTINGS_SAVE_FOLDER = "Assets/Resources/NoiseSettings/";

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
			NoiseSettings noiseSettings = new NoiseSettings();
			NativeArray<ModuleData> data = BurstModuleManager.CreateModuleData(moduleBase);
			noiseSettings.moduleData = data.ToArray();
			data.Dispose();
			AssetDatabase.CreateAsset (noiseSettings, NOISE_SETTINGS_SAVE_FOLDER + name.Replace(" ", "") + ".asset");
        	AssetDatabase.SaveAssets ();
		}
	}
}