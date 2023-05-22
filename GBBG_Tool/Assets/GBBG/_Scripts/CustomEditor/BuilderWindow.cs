using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuilderWindow : EditorWindow
{


    [MenuItem("Window/GBBG/Builder")]
    public static void ShowWindow()
    {
        GetWindow(typeof(BuilderWindow), false, "Builder");
    }

	private void OnGUI()
	{
		
	}
}
