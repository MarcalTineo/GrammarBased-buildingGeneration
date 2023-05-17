using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GBBG
{
	public static class EditorUtilities
	{
		/// <summary>
		/// Draws an editor GUI for lists of Objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list</param>
		/// <param name="name">The name shown in the header</param>
		/// <param name="expandFoldout">To track if the foldout is expanded</param>
		/// <returns>The updated list</returns>
		public static List<T> DrawObjectList<T>(List<T> list, GUIContent name, ref bool expandFoldout, bool disableEditSize, List<GUIContent> labels) where T : UnityEngine.Object
		{
			int count = list.Count;
			EditorGUILayout.BeginHorizontal();
			expandFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(expandFoldout, name);
			EditorGUI.BeginDisabledGroup(disableEditSize);
			count = EditorGUILayout.DelayedIntField(count, GUILayout.Width(50));
			if (count != list.Count)
				list.Resize(count, null);
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.EndHorizontal();
			if (expandFoldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				for (int i = 0; i < list.Count; i++)
				{
					if (labels == null)
						list[i] = (T)EditorGUILayout.ObjectField(list[i], typeof(T), false);
					else
						list[i] = (T)EditorGUILayout.ObjectField(labels[i], list[i], typeof(T), false);
				}
				EditorGUILayout.EndVertical();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFoldoutHeaderGroup();
			return list;
		}

		/// <summary>
		/// Draws an editor GUI for lists of Objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list</param>
		/// <param name="name">The name shown in the header</param>
		/// <param name="expandFoldout">To track if the foldout is expanded</param>
		/// <returns>The updated list</returns>
		public static List<T> DrawObjectList<T>(List<T> list, GUIContent name, ref bool expandFoldout, List<GUIContent> labels, bool disableEditSize = false) where T : UnityEngine.Object
		{
			return DrawObjectList<T>(list, name, ref expandFoldout, disableEditSize, labels);
		}

		/// <summary>
		/// Draws an editor GUI for lists of Objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list</param>
		/// <param name="name">The name shown in the header</param>
		/// <param name="expandFoldout">To track if the foldout is expanded</param>
		/// <returns>The updated list</returns>
		public static List<T> DrawObjectList<T>(List<T> list, string name, ref bool expandFoldout, List<GUIContent> labels, bool disableEditSize = false) where T : UnityEngine.Object
		{
			return DrawObjectList<T>(list, new GUIContent(name), ref expandFoldout, disableEditSize, labels);
		}

		/// <summary>
		/// Draws an editor GUI for lists of Objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list</param>
		/// <param name="name">The name shown in the header</param>
		/// <param name="expandFoldout">To track if the foldout is expanded</param>
		/// <returns>The updated list</returns>
		public static List<T> DrawObjectList<T>(List<T> list, GUIContent name, ref bool expandFoldout, bool disableEditSize = false) where T : UnityEngine.Object
		{
			return DrawObjectList<T>(list, name, ref expandFoldout, null, disableEditSize);
		}

		/// <summary>
		/// Draws an editor GUI for lists of Objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list</param>
		/// <param name="name">The name shown in the header</param>
		/// <param name="expandFoldout">To track if the foldout is expanded</param>
		/// <returns>The updated list</returns>
		public static List<T> DrawObjectList<T>(List<T> list, string name, ref bool expandFoldout, bool disableEditSize = false) where T : UnityEngine.Object
		{
			return DrawObjectList<T>(list, new GUIContent(name), ref expandFoldout, null, disableEditSize);
		}



		/// <summary>
		/// Draws an editor GUI for lists of floats
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list</param>
		/// <param name="name">The name shown in the header</param>
		/// <param name="expandFoldout">To track if the foldout is expanded</param>
		/// <returns>The updated list</returns>
		public static List<float> DrawFloatList(List<float> list, GUIContent name, ref bool expandFoldout, bool disableEditSize = false)
		{
			int count = list.Count;
			EditorGUILayout.BeginHorizontal();
			expandFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(expandFoldout, name);
			EditorGUI.BeginDisabledGroup(disableEditSize);
			count = EditorGUILayout.DelayedIntField(count, GUILayout.Width(50));
			if (count != list.Count)
				list.Resize(count);
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.EndHorizontal();
			if (expandFoldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				for (int i = 0; i < list.Count; i++)
				{
					list[i] = EditorGUILayout.FloatField(list[i]);
				}
				EditorGUILayout.EndVertical();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFoldoutHeaderGroup();
			return list;
		}

		/// <summary>
		/// Draws an editor GUI for lists of floats
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list</param>
		/// <param name="name">The name shown in the header</param>
		/// <param name="expandFoldout">To track if the foldout is expanded</param>
		/// <returns>The updated list</returns>
		public static List<float> DrawFloatList(List<float> list, string name, ref bool expandFoldout, bool disableEditSize = false)
		{
			return DrawFloatList(list, new GUIContent(name), ref expandFoldout, disableEditSize);
		}

		/// <summary>
		/// Sets new size for the list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="size"></param>
		/// <param name="type"></param>
		public static void Resize<T>(this List<T> list, int size, T type)
		{
			int current = list.Count;
			if (size < current)
				list.RemoveRange(size, current - size);
			else if (size > current)
			{
				if (size > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
					list.Capacity = size;
				list.AddRange(Enumerable.Repeat(type, size - current));
			}
		}
		public static void Resize<T>(this List<T> list, int size) where T : new()
		{
			Resize(list, size, new T());
		}


		/// <summary>
		/// Renames name and asset file name of an scriptable object rule.
		/// </summary>
		/// <param name="rule"></param>
		/// <param name="newName"></param>
		/// <returns>An empty string, if the asset has been successfully renamed, otherwise an error message.</returns>
		public static string RenameRule(this Rule rule, string newName)
		{
			//get asset path
			string path = AssetDatabase.GetAssetPath(rule.GetInstanceID());

			//save unsaved changes
			AssetDatabase.SaveAssetIfDirty(AssetDatabase.GUIDFromAssetPath(path));

			//rename asset
			string result = AssetDatabase.RenameAsset(path, newName);

			//save assets
			AssetDatabase.SaveAssets();

			//change so name
			rule.name = newName;
			return result;
		}




		//---------------------------------------------------- no usat
		/// <summary>
		/// Draws a warning label
		/// </summary>
		/// <param name="message"></param>
		public static void DrawAlert(string message)
		{
			GUILayout.BeginHorizontal("box");
			int size = 20;
			GUILayout.Box(EditorGUIUtility.FindTexture("console.warnicon"), GUILayout.Width(size), GUILayout.Height(size));
			GUILayout.Label(message);
			GUILayout.EndHorizontal();
		}
		//get the rect for the next property to display
		public static Rect GetNext()
		{
			Rect rect = GUILayoutUtility.GetLastRect();
			rect.y += GUILayoutUtility.GetLastRect().height + EditorGUIUtility.standardVerticalSpacing;
			return rect;
		}
	}
}
