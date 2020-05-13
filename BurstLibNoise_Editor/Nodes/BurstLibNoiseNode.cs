using UnityEngine;
using BurstLibNoise;
using UnityEditor;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	public abstract class BurstLibNoiseNode : Node
	{
		public const string NOISE_SETTINGS_SAVE_FOLDER = "Assets/BurstLibNoise/BurstLibNoise/NoiseSettings/";

		public override Vector2 DefaultSize { get { return new Vector2(120, 230); } }
		public override Vector2 MinSize { get { return new Vector2(130, 30); } }
		public override bool AutoLayout { get { return true; } }
        
		[System.NonSerialized]
		public Texture2D tex;

		private int width = 120;
		private int height = 120;

        public Texture2D defaultTex;

        public abstract void BurstLibNoiseNodeGUI();

        protected void DrawTexture() {
            Texture texToDraw;
			if (tex != null) {
                texToDraw = tex;
			} else {
                if (defaultTex == null) {
                    defaultTex = new Texture2D(width, height);
                }
                texToDraw = defaultTex;
            }
            GUIStyle texStyle = new GUIStyle();
            texStyle.alignment = TextAnchor.UpperCenter;
            RTTextureViz.DrawTexture(texToDraw, width);
            GUILayout.Space(5);
        }

		public override void NodeGUI()
		{
			backgroundColor = Color.grey;

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			EditorGUIUtility.labelWidth = 70;
			EditorGUIUtility.fieldWidth = 20;

			BurstLibNoiseNodeGUI();

			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			if (GUI.changed)
				NodeEditor.curNodeCanvas.OnNodeChange(this);
		}

		protected Texture2D GenerateTex(BurstModuleBase module) {
			Noise2D heightMapBuilder = new Noise2D(width, height, module);
			heightMapBuilder.GeneratePlanar(-1, 1, -1, 1);
			Texture2D heightMap = heightMapBuilder.GetTexture();
			heightMapBuilder.Dispose();
			return heightMap;
		}
	}
}