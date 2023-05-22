using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GBBG
{
    public class TerminalShapeCreator : MonoBehaviour
    {
		private static TerminalShapeCreator instance;
		public static TerminalShapeCreator GetInstance()
		{
			if (instance == null)
			{
				GameObject go = new GameObject();
				TerminalShapeCreator tsc = go.AddComponent<TerminalShapeCreator>();
				instance = tsc;
			}
			return instance;
		}

		
	}

}
