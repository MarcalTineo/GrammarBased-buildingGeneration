using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GBBG
{
    public class TerminalShapeCreatorWindow : EditorWindow
    {
        int step;
        int totalSteps = 5;

		GameObject pivotArea;
		Vector3 pivotAreaPosition;

		GameObject terminalObject;

		GameObject sizeArea;
		Vector3 sizeAreaScale;


        string[] descriptions = new string[6]
        {
            "Set the pivot point in the scene where you want to. You also can drag the gameObject <Pivot Area Marker>.",
            "Drag from the scene hierarchy the gameobject you want to convert to a terminal shape.",
            "Adjust the selected gameObject to the pivot. Position, rotation and scale must be correct.",
            "",
            "",
            ""
        };

        [MenuItem("Window/GBBG/Terminal Shape Creator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(TerminalShapeCreatorWindow), false, "Terminal Shape Creator");
            
        }

		private void OnEnable()
		{
			step = 0;
			CreatePivotArea();
		}

		private void OnDisable()
		{
			DestroyImmediate(pivotArea);
		}

		private void OnGUI()
		{
            ///steps
            ///1. Set pivot point of workspace
            ///2. Select Gameobjct
            ///3. Adjust gameobject to pivot
            ///5. Define area
            ///6. Bake
            ///

            DrawTopBar();
            EditorGUILayout.Space(5);
            DrawInfo();
            EditorGUILayout.Space(5);
            switch (step)
            {
                case 0:
                    Step0();
                    break;
				case 1:
					Step1();
					break;
				case 2:
					Step2();
					break;
				case 3:
					Step3();
					break;
				case 4:
					Step4();
					break;
				case 5:
					Step5();
					break;
                default: 
                    break; 
			}

		}

		private void DrawTopBar()
		{
			EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(step <= 0);
            if (GUILayout.Button("Previous Step", GUILayout.Width(100)))
                PreviousStep();
			EditorGUI.EndDisabledGroup();

            GUIStyle style = EditorStyles.boldLabel;
            style.alignment = TextAnchor.LowerCenter;
            GUILayout.Label($"Step {step}", style);

			EditorGUI.BeginDisabledGroup(step >= totalSteps);
            if (GUILayout.Button("Next Step", GUILayout.Width(100)))
                NextStep();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
		}

		private void DrawInfo()
		{
			EditorGUILayout.BeginVertical("box");
			GUIStyle style = EditorStyles.label;
			style.alignment = TextAnchor.UpperLeft;
			style.wordWrap = true;
			GUILayout.Label(descriptions[step], style);
			EditorGUILayout.EndVertical();
		}


		#region StepControl
		private void NextStep()
		{
            step++;
            switch (step)
            {
				case 1:
					ExitStep0();
                    EnterStep1();
					break;
				case 2:
					ExitStep1();
					EnterStep2();
					break;
				case 3:
					ExitStep2();
					EnterStep3();
					break;
				case 4:
					ExitStep3();
					EnterStep4();
					break;
				case 5:
					ExitStep4();
					EnterStep5();
					break;
				default:
					break;
			}
		}

		private void PreviousStep()
		{
			step--;
			switch (step)
			{
				case 0:
					ExitStep1();
					EnterStep0();
					break;
				case 1:
					ExitStep2();
					EnterStep1();
					break; 
				case 2:
					ExitStep3();
					EnterStep2();
					break;
				case 3:
					ExitStep4();
					EnterStep3();
					break;
				case 4:
					ExitStep5();
					EnterStep4();
					break;
				default:
					break;
			}
		}

		#endregion

		#region step 0

		private void Step0()
		{
			if(pivotArea != null)
			{
				pivotAreaPosition = pivotArea.transform.position;
				pivotAreaPosition = EditorGUILayout.Vector3Field("Position", pivotAreaPosition);
				Repaint();
				if(GUILayout.Button("Snap to Grid"))
				{
					pivotAreaPosition.x = Mathf.Round(pivotAreaPosition.x);
					pivotAreaPosition.y = Mathf.Round(pivotAreaPosition.y);
					pivotAreaPosition.z = Mathf.Round(pivotAreaPosition.z);

				}
				pivotArea.transform.position = pivotAreaPosition;
			}
			else
			{
				if (GUILayout.Button("Respawn Pivot Marker"))
				{
					if (pivotArea == null)
					{
						CreatePivotArea();
					}
				}
				EditorGUILayout.HelpBox("Don't delete <Pivot Area Marker> GameObject.", MessageType.Error);
			}
			
		}

		private void EnterStep0()
		{
			if (pivotArea == null)
				CreatePivotArea();
			pivotArea.SetActive(true);
		}

		private void ExitStep0()
		{
			if (pivotArea == null)
				CreatePivotArea();
			pivotArea.SetActive(false);
		}

		private void CreatePivotArea()
		{
			pivotArea = new GameObject("Pivot Area Marker (do not destroy)");
			pivotArea.AddComponent<PivotAreaMarker>();
			pivotArea.transform.position = pivotAreaPosition;
		}

		#endregion
		#region step 1
		private void Step1()
		{
			terminalObject = (GameObject)EditorGUILayout.ObjectField("Terminal Object", terminalObject, typeof(GameObject), true);
		}

		private void EnterStep1()
		{

		}

		private void ExitStep1()
		{
			if(terminalObject == null)
			{
				Debug.LogWarning("No gameObject Selected");
			}
		}
		#endregion
		#region step 2
		private void Step2()
		{

		}

		private void EnterStep2()
		{
			if (pivotArea == null)
				CreatePivotArea();
			pivotArea.SetActive(true);
		}

		private void ExitStep2()
		{
			if (pivotArea == null)
				CreatePivotArea();
			pivotArea.SetActive(false);
		}
		#endregion
		#region step 3
		private void Step3()
		{
			if(sizeArea == null)
			{
				sizeAreaScale = sizeArea.transform.localScale;
				sizeAreaScale = EditorGUILayout.Vector3Field(new GUIContent("Scale"), sizeAreaScale);
				sizeArea.transform.localScale = sizeAreaScale;
				Repaint();
			}
			else
			{

			}
		}

		private void EnterStep3()
		{

		}

		private void ExitStep3()
		{

		}

		private void CreateSizeArea()
		{
			sizeArea = new GameObject("Size Area (do not destroy)");
			sizeArea.AddComponent<>();
		}

		#endregion
		#region step 4
		private void Step4()
		{

		}

		private void EnterStep4()
		{

		}

		private void ExitStep4()
		{

		}
		#endregion
		#region step 5
		private void Step5()
		{

		}

		private void EnterStep5()
		{

		}

		private void ExitStep5()
		{

		}
		#endregion

	}

}
