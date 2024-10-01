using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[System.Serializable]
public class PostProcess
{
	public string symbol;
	public GameObject asset;
}

public class MyScriptableObject : ScriptableObject
{
	//public List<string> strings = new List<string>();
	//public Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();
	public List<PostProcess> objects = new List<PostProcess>();
}
public class MyEditorWindow : EditorWindow
{
	ReorderableList strings_ro_list;
	SerializedObject serializedObject;
	SerializedProperty stringsProperty;
	bool b;

	SerializedProperty objectsPropery;




	[MenuItem("Window/Test Editor Window")]
	public static void ShowWindow()
	{
		GetWindow<MyEditorWindow>("Test Editor");
	}
	private void OnEnable()
	{
		MyScriptableObject obj = ScriptableObject.CreateInstance<MyScriptableObject>();

		serializedObject = new UnityEditor.SerializedObject(obj);
		stringsProperty = serializedObject.FindProperty("objects");
		//objectsPropery = serializedObject.FindProperty("objects");
		strings_ro_list = new ReorderableList(serializedObject, stringsProperty, true, true, true, true);
		strings_ro_list.drawElementCallback = StringsDrawListItems;
		strings_ro_list.drawHeaderCallback = StringsDrawHeader;





	}
	private void OnGUI()
	{
		if (this.serializedObject == null)
		{
			return;
		}
		serializedObject.Update();
		strings_ro_list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();


		//EditorGUILayout.PropertyField(objectsPropery, new GUIContent("Prova"), true);


		//serializedObject.ApplyModifiedProperties();

	}
	void StringsDrawListItems(Rect rect, int index, bool isActive, bool isFocused)
	{
		// your GUI code here for list content
		var element = stringsProperty.GetArrayElementAtIndex(index);
		//element.symbol 

		EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width / 2, rect.height), new GUIContent("Symbol"));
		//EditorGUI.ObjectField(new Rect(rect.x + rect.width/2, rect.y, rect.width / 2, rect.height), )
	}
	void StringsDrawHeader(Rect rect)
	{
		// your GUI code here for list header
		//b = EditorGUILayout.BeginFoldoutHeaderGroup(b, stringsProperty.name);


		//EditorGUILayout.EndFoldoutHeaderGroup();
	}
}