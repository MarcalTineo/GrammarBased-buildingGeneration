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
		const string assetPath = "Assets/Resources/ShapePefabs/Shape 3D.prefab";
		AssetDatabase.CopyAsset(assetPath, $"{CurrentProjectFolderPath}/New Shape 3D.prefab");
	}

	[MenuItem("Assets/Create/GBBG/Shape/2D")]
	public static void CreateShape2D()
	{
		const string assetPath = "Assets/Resources/ShapePefabs/Shape 2D.prefab";
		AssetDatabase.CopyAsset(assetPath, $"{CurrentProjectFolderPath}/New Shape 2D.prefab");
	}
	[MenuItem("Assets/Create/GBBG/Shape/Empty")]
	public static void CreateShapeEmpty()
	{
		const string assetPath = "Assets/Resources/ShapePefabs/Empty.prefab";
		AssetDatabase.CopyAsset(assetPath, $"{CurrentProjectFolderPath}/New Empty Shape.prefab");
	}

}


