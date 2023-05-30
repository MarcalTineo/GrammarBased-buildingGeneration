using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CreateShapeEditor : MonoBehaviour
{
	public static string CurrentProjectFolderPath
	{
		get
		{
			Type projectWindowUtilType = typeof(ProjectWindowUtil);
			MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
			object obj = getActiveFolderPath.Invoke(null, new object[0]);
			return obj.ToString();
		}
	}

	[MenuItem("Assets/Create/GBBG/Shape/3D")]
	public static void CreateShape3D()
	{
		const string assetPath = "Assets/GBBG/Prefabs/Shape 3D.prefab";
		bool result = AssetDatabase.CopyAsset(assetPath, $"{CurrentProjectFolderPath}/New Shape 3D.prefab");
		if (!result) 
		{
			Debug.LogError("Failed to copy asset to " + CurrentProjectFolderPath + ".");
		}
	}

	[MenuItem("Assets/Create/GBBG/Shape/2D")]
	public static void CreateShape2D()
	{
		const string assetPath = "Assets/GBBG/Prefabs/Shape 2D.prefab";
		bool result = AssetDatabase.CopyAsset(assetPath, $"{CurrentProjectFolderPath}/New Shape 2D.prefab");
		if (!result)
		{
			Debug.LogError("Failed to copy asset to " + CurrentProjectFolderPath + ".");
		}
	}

	[MenuItem("Assets/Create/GBBG/Shape/Empty")]
	public static void CreateShapeEmpty()
	{
		const string assetPath = "Assets/GBBG/Prefabs/Empty.prefab";
		bool result = AssetDatabase.CopyAsset(assetPath, $"{CurrentProjectFolderPath}/New Empty Shape.prefab");
		if (!result)
		{
			Debug.LogError("Failed to copy asset to " + CurrentProjectFolderPath + ".");
		}
	}

}


