using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GBBG
{
	public class BuilderWindow : EditorWindow
	{
		SerializedObject serializedObject;
		SerializedProperty axiomProperty;
		public List<Shape> axiom = new List<Shape>();
		Grammar grammar;
		bool doPostProduction;

		[MenuItem("Window/GBBG/Builder")]
		public static void ShowWindow()
		{
			GetWindow(typeof(BuilderWindow), false, "Builder");
		}

		private void OnEnable()
		{
			serializedObject = new SerializedObject(this);
			axiomProperty = serializedObject.FindProperty("axiom");
		}

		private void OnGUI()
		{
			GUILayout.Space(5);
			EditorGUILayout.PropertyField(axiomProperty, new GUIContent("Axiom", "The start point od the derivation"));
			serializedObject.ApplyModifiedProperties();
			GUILayout.Space(5);
			grammar = (Grammar)EditorGUILayout.ObjectField(new GUIContent("Grammar"), grammar, typeof(Grammar), false);

			doPostProduction = EditorGUILayout.BeginToggleGroup(new GUIContent("Do Post Production"), doPostProduction);
			
			EditorGUILayout.EndToggleGroup();

			bool canBuild = grammar != null;
			canBuild = canBuild && axiom.Count > 0;
			if (canBuild)
				canBuild =  axiom[0] != null;

			EditorGUI.BeginDisabledGroup(!canBuild);
			if (GUILayout.Button(new GUIContent("Build"))) 
			{
				DerivationBuilder.GetInstance().Build(axiom, grammar, doPostProduction);
			}
			EditorGUI.EndDisabledGroup();

		}
	}
}
