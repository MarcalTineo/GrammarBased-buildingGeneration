using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GBBG
{
	[CustomEditor(typeof(Builder))]
	public class BuilderCustomInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			Builder b = (Builder)target;
			if (GUILayout.Button("Build"))
			{
				b.Build();
			}
			if (GUILayout.Button("Reset"))
			{
				b.Reset();
			}
			DrawDefaultInspector();
		}

	}
}