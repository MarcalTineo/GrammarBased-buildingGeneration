using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GBBG
{
    public class TerminalShapeCreator : EditorWindow
    {
        int step;
        int totalSteps = 5;

		GameObject pivotArea;
		Vector3 pivotAreaPosition;

		GameObject terminalObject;

		GameObject sizeArea;
		Vector3 sizeAreaScale;
		public GameObject sizeAreaPrefab;

		string terminalShapeName = "";
		bool shapeCreated;


        string[] descriptions = new string[6]
        {
            "Set the pivot point in the scene where you want to. You also can drag the gameObject <Pivot Area Marker>.",
            "Drag from the scene hierarchy the gameobject you want to convert to a terminal shape.",
            "Adjust the selected gameObject to the pivot. Position, rotation and scale must be correct.",
            "Adjust the volume that ocupies the shape. This should be the volume of the terminal shape representing the asset.",
            "Choose a name for the terminal shape",
            "The create button will adjust all things for this asset to be used in the post-production process."
        };

        [MenuItem("Window/GBBG/Terminal Shape Creator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(TerminalShapeCreator), false, "Terminal Shape Creator");
            
        }

		private void OnEnable()
		{
			step = 0;
			if (pivotArea == null) 
				CreatePivotArea();
			if (sizeArea == null)
				CreateSizeArea();
			sizeArea.SetActive(false);
			pivotArea.SetActive(false);
			EnterStep0();
			shapeCreated = false;
			terminalObject = null;
			terminalShapeName = "";
		}

		private void OnDisable()
		{
			DestroyImmediate(pivotArea);
			DestroyImmediate(sizeArea);
		}

		private void OnGUI()
		{
            ///steps
            ///1. Set pivot point of workspace
            ///2. Select Gameobjct
            ///3. Adjust gameobject to pivot
            ///4. Define area
			///5. name
            ///6. Bake
            ///

            DrawTopBar();
            EditorGUILayout.Space(5);
            EditorUtilities.DrawInfo(descriptions[step]);
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
            GUILayout.Label($"Step {step + 1}", style);

			EditorGUI.BeginDisabledGroup(step >= totalSteps);
            if (GUILayout.Button("Next Step", GUILayout.Width(100)))
                NextStep();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
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
			if(terminalObject == null)
			{
				EditorGUILayout.HelpBox("No GameObject selected.", MessageType.Warning);
			}
		}

		private void EnterStep1()
		{

		}

		private void ExitStep1()
		{
			if(terminalObject == null)
			{
				Debug.LogWarning("No gameObject Selected");
				PreviousStep();
			}
		}
		#endregion
		#region step 2
		private void Step2()
		{
			terminalObject.transform.localPosition = EditorGUILayout.Vector3Field(new GUIContent("Position", "The local position of the selected GameObject."), terminalObject.transform.localPosition);
			terminalObject.transform.localEulerAngles = EditorGUILayout.Vector3Field(new GUIContent("Rotation", "The local euler angles of the selected GameObject."), terminalObject.transform.localEulerAngles);
			terminalObject.transform.localScale = EditorGUILayout.Vector3Field(new GUIContent("Scale", "The local scale of the selected GameObject."), terminalObject.transform.localScale);


			GUILayout.Space(10);
			if(GUILayout.Button("Set position to pivot"))
			{
				terminalObject.transform.position = pivotAreaPosition;
			}
			
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
			if(sizeArea != null)
			{
				sizeAreaScale = sizeArea.transform.localScale;
				sizeAreaScale = EditorGUILayout.Vector3Field(new GUIContent("Scale"), sizeAreaScale);
				sizeArea.transform.localScale = sizeAreaScale;
				Repaint();
				if (GUILayout.Button("Snap to integers"))
				{
					sizeArea.transform.localScale = new Vector3(Mathf.Round(sizeAreaScale.x), Mathf.Round(sizeAreaScale.y), Mathf.Round(sizeAreaScale.z));
				}
			}
			else
			{
				CreateSizeArea();
			}
			
		}

		private void EnterStep3()
		{
			if (sizeArea == null)
				CreateSizeArea();
			sizeArea.SetActive(true);

			if (pivotArea == null)
				CreatePivotArea();
			pivotArea.SetActive(true);

			if(sizeAreaScale == Vector3.zero)
			{
				sizeAreaScale = Vector3.one;
				sizeArea.transform.localScale = sizeAreaScale;
			}
		}

		private void ExitStep3()
		{
			if (sizeArea == null)
				CreateSizeArea();
			sizeArea.SetActive(false);

			if (pivotArea == null)
				CreatePivotArea();
			pivotArea.SetActive(false);
		}

		private void CreateSizeArea()
		{
			sizeArea = Instantiate(sizeAreaPrefab);
			sizeArea.name = "Size Area Marker (do not destroy)";
			sizeArea.transform.position = pivotAreaPosition;
			sizeArea.transform.localScale = sizeAreaScale;
		}

		
		#endregion
		#region step 4
		private void Step4()
		{
			terminalShapeName = EditorGUILayout.TextField(new GUIContent("Name", "The name of the created shape"), terminalShapeName);
		}


		private void EnterStep4()
		{
			
		}

		private void ExitStep4()
		{
			if(terminalShapeName == "")
			{
				PreviousStep();
			}
		}
		#endregion
		#region step 5

		private void Step5()
		{
			EditorGUI.BeginDisabledGroup(shapeCreated);
			if (GUILayout.Button("Create"))
			{
				if (terminalObject != null)
				{
					shapeCreated = true;
					CreateShape();
					ExitStep5();
				}
			}
			EditorGUI.EndDisabledGroup();
			if (shapeCreated)
			{
				EditorUtilities.DrawInfo($"Please, save the object named <{terminalShapeName} (terminal shape)> created as a perfab to use in the post-production.");
				if (GUILayout.Button("Create New Shape"))
				{
					OnEnable();
				}

			}
		}

		private void EnterStep5()
		{
			if (sizeArea == null)
				CreateSizeArea();
			sizeArea.SetActive(true);

			if (pivotArea == null)
				CreatePivotArea();
			pivotArea.SetActive(true);
		}

		private void ExitStep5()
		{
			if (sizeArea == null)
				CreateSizeArea();
			sizeArea.SetActive(false);

			if (pivotArea == null)
				CreatePivotArea();
			pivotArea.SetActive(false);
		}

		private void CreateShape()
		{
			GameObject root = new GameObject($"{terminalShapeName} (terminal shape)");
			root.transform.position = pivotAreaPosition;

			terminalObject.transform.parent = root.transform;
			terminalObject.transform.localScale = new Vector3(1 / sizeAreaScale.x, 1 / sizeAreaScale.y, 1 / sizeAreaScale.z);
		}
		#endregion

	}

}
