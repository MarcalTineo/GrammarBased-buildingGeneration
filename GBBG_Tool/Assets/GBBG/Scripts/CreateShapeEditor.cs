using GBBG;
using UnityEditor;

public class CreateShapeEditor
{
	
	const string shape3DAssetPath = "Assets/GBBG/Prefabs/Shape 3D.prefab";
	const string shape2DAssetPath = "Assets/GBBG/Prefabs/Shape 2D.prefab";
	const string emptyShapeAssetPath = "Assets/GBBG/Prefabs/Empty.prefab";

	public static string Shape3DAssetPath => shape3DAssetPath;
	public static string Shape2DAssetPath => shape2DAssetPath;
	public static string EmptyShapeAssetPath => emptyShapeAssetPath;

	[MenuItem("Assets/Create/GBBG/Shape/3D")]
	public static void CreateShape3D()
	{
		EditorUtilities.CopyPrefab(shape3DAssetPath, "New Shape 3D", EditorUtilities.CurrentProjectFolderPath);
	}

	[MenuItem("Assets/Create/GBBG/Shape/2D")]
	public static void CreateShape2D()
	{
		EditorUtilities.CopyPrefab(shape2DAssetPath, "New Shape 2D", EditorUtilities.CurrentProjectFolderPath);
	}

	[MenuItem("Assets/Create/GBBG/Shape/Empty")]
	public static void CreateShapeEmpty()
	{
		EditorUtilities.CopyPrefab(emptyShapeAssetPath, "New Empty Shape", EditorUtilities.CurrentProjectFolderPath);
		
	}

}


