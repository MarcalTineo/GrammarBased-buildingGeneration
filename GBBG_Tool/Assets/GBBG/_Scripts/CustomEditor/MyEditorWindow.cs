using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class MyScriptableObject : ScriptableObject
{
	public List<string> strings = new List<string>();
}
public class MyEditorWindow : EditorWindow
{
	ReorderableList strings_ro_list;
	SerializedObject serializedObject;
	SerializedProperty stringsProperty;
	bool b;
	[MenuItem("Window/Test Editor Window")]
	public static void ShowWindow()
	{
		GetWindow<MyEditorWindow>("Test Editor");
	}
	private void OnEnable()
	{
		MyScriptableObject obj = ScriptableObject.CreateInstance<MyScriptableObject>();

		serializedObject = new UnityEditor.SerializedObject(obj);
		stringsProperty = serializedObject.FindProperty("strings");

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
	}
	void StringsDrawListItems(Rect rect, int index, bool isActive, bool isFocused)
	{
		// your GUI code here for list content
	}
	void StringsDrawHeader(Rect rect)
	{
		// your GUI code here for list header
		b = EditorGUILayout.BeginFoldoutHeaderGroup(b, stringsProperty.name);
	}
}