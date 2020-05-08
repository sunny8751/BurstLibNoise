using System;
using UnityEngine;
using BurstLibNoise;

namespace NodeEditorFramework.BurstLibNoiseEditor
{
	public class ModuleType : ValueConnectionType
	{
		public override string Identifier { get { return "BurstModuleBase"; } }
		public override Type Type { get { return typeof(BurstModuleBase); } }
		public override Color Color { get { return Color.green; } }
	}
}