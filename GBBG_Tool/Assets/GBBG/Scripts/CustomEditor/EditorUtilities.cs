using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GBBG
{
	public static class EditorUtilities
	{
		public static Dictionary<string, bool> expandFoldout = new Dictionary<string, bool>();
		
		public static bool GetExpand(string s)
		{
			if (expandFoldout.ContainsKey(s))
				return expandFoldout[s];
			else
				return false;
		}

		public static void SetExpand(string s, bool b)
		{
			if(expandFoldout.ContainsKey(s))
				expandFoldout[s] = b;
			else
				expandFoldout.Add(s, b);
		}

		/// <summary>
		/// Draws a list of successors
		/// </summary>
		/// <param name="list"></param>
		/// <param name="name"></param>
		/// <param name="expandFoldout"></param>
		/// <returns></returns>
		public static List<Successor> DrawSuccessorList(List<Successor> list, GUIContent name, ref bool expandFoldout)
		{
			int count = list.Count;
			GUILayout.BeginHorizontal();
			expandFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(expandFoldout, name);
			count = EditorGUILayout.DelayedIntField(count, GUILayout.Width(50));
			count = Mathf.Max(1, count);
			if (count != list.Count)
				list.Resize(count);
			GUILayout.EndHorizontal();
			if (expandFoldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				for (int i = 0; i < list.Count; i++)
				{
					list[i] = DrawSuccessorField(list[i]);
				}
				EditorGUILayout.EndVertical();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFoldoutHeaderGroup();
			return list;
		}

		public static List<Successor> DrawLabeledSuccessorList (List<Successor> list, GUIContent name, ref bool expandFoldout, List<GUIContent> labels)
		{
			if (labels.Count < 1)
			{
				Debug.LogError("No labels set");
				return null;
			}
			int count = labels.Count;
			GUILayout.BeginHorizontal();
			expandFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(expandFoldout, name);
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.DelayedIntField(count, GUILayout.Width(50));
			EditorGUI.EndDisabledGroup();
			if (count != list.Count)
				list.Resize(3);

			GUILayout.EndHorizontal();
			if (expandFoldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				for (int i = 0; i < list.Count; i++)
				{
					GUILayout.Label(labels[i]);
					list[i] = DrawSuccessorField(list[i]);
				}
				EditorGUILayout.EndVertical();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFoldoutHeaderGroup();
			return list;
		}
		public static List<Successor> DrawLabeledSuccessorList(List<Successor> list, string name, ref bool expandFoldout, List<GUIContent> labels)
		{
			return DrawLabeledSuccessorList(list, new GUIContent(name), ref expandFoldout, labels);
		}

		/// <summary>
		/// Draws a field for successor on inspector
		/// </summary>
		/// <param name="s">The values of the successor to be displayed</param>
		/// <returns>The actualized successor</returns>
		public static Successor DrawSuccessorField(Successor s)
		{
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			float lw = EditorGUIUtility.labelWidth;
			int count = s.possibleSuccessors.Count;

			count = EditorGUILayout.DelayedIntField(new GUIContent("Possibilities"), count);
			count = Mathf.Max(1, count);
			if (count != s.possibleSuccessors.Count)
				s.possibleSuccessors.Resize(count, new SuccessorData());

			EditorGUIUtility.labelWidth = 100;
			foreach (SuccessorData data in s.possibleSuccessors)
			{
				Rect r = EditorGUILayout.BeginHorizontal();
				data.chance = EditorGUILayout.FloatField(new GUIContent("Chance"), data.chance);
				data.successor = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Successor"), data.successor, typeof(GameObject), false);
				EditorGUILayout.EndHorizontal();
				if (data.chance == 0) EditorGUILayout.HelpBox("Chance 0 may cause errors.", MessageType.Warning);
			}
			EditorGUIUtility.labelWidth = lw;
			EditorGUILayout.EndVertical();
			return s;
		}

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
		public static void ResizeSOList<T>(this List<T> list, int size) where T : ScriptableObject
		{
			int current = list.Count;
			if (size < current)
				list.RemoveRange(size, current - size);
			else if (size > current)
			{
				if (size > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
					list.Capacity = size;
				for (int i = current; i < size; i++)
				{
					list.Add(ScriptableObject.CreateInstance(typeof(T)) as T);
				}
			}
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
			//AssetDatabase.SaveAssetIfDirty(AssetDatabase.GUIDFromAssetPath(path));

			//rename asset
			string result = AssetDatabase.RenameAsset(path, newName);

			//save assets
			//AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			//change so name
			//rule.name = newName;
			return result;
		}


		/// <summary>
		/// Copies an existing prefab to the given loaction.
		/// </summary>
		/// <param name="fromPath">The path of the asset to copy. Must include name and extension of the prefab</param>
		/// <param name="name">The name of new prefab.</param>
		/// <param name="toPath">The folder where the prefab will be copied to.</param>
		/// <returns>True: all worked. False: someting failed.</returns>
		public static string CopyPrefab(string fromPath, string name, string toPath)
		{
			if (!AssetDatabase.IsValidFolder(toPath))
			{
				Debug.LogError($"The path {toPath} does not exist.");
				return null;
			}
			bool shapeCreated = false;
			bool result = true;
			int difIndex = 0;

			string newAssetPath = null;
			while (!shapeCreated)
			{
				newAssetPath = toPath + "/" + name + (difIndex == 0 ? "" : $" {difIndex}") + ".prefab";
				if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(newAssetPath)))
				{
					result = AssetDatabase.CopyAsset(fromPath, newAssetPath);
					shapeCreated = true;
				}
				difIndex++;
			}
			if (!result)
			{
				Debug.LogError("Failed to copy asset to " + toPath + ".");
				return null;
			}

			return newAssetPath;
		}


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

		

		/// <summary>
		/// Draws a text box
		/// </summary>
		/// <param name="info"></param>
		public static void DrawInfo(string info)
		{
			EditorGUILayout.BeginVertical("box");
			GUIStyle style = EditorStyles.label;
			style.alignment = TextAnchor.UpperLeft;
			style.wordWrap = true;
			GUILayout.Label(info, style);
			EditorGUILayout.EndVertical();
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
