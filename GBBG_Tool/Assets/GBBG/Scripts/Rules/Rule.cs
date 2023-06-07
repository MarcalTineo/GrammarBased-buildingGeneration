using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;

namespace GBBG
{
	[System.Serializable]
	public class Rule : ScriptableObject
	{
		//public enum Axis { X, Y, Z };
		public string predescesor;
		public List<Successor> succesor;

		public virtual List<Shape> ApplyRule(Shape shape) { return null; }

		public virtual string GetRuleNotation() { return null; }

		public virtual void Init() 
		{
			succesor = new List<Successor>
			{
				new Successor()
			};
			EditorUtility.SetDirty(this);
		}
		

		public GameObject CreateNewShape(GameObject newShape, Transform parent)
		{
			newShape = Instantiate(newShape);
			newShape.name = newShape.GetComponent<Shape>().Symbol;
			newShape.transform.position = parent.transform.position;
			newShape.transform.rotation = parent.transform.rotation;
			newShape.transform.localScale = parent.transform.localScale;
			return newShape;
		}

		public Shape CreateNewShape2(GameObject newShape, Transform parent)
		{
			newShape = Instantiate(newShape);
			newShape.name = newShape.GetComponent<Shape>().Symbol;
			newShape.transform.position = parent.transform.position;
			newShape.transform.rotation = parent.transform.rotation;
			newShape.transform.localScale = parent.transform.localScale;
			return newShape.GetComponent<Shape>();
		}

		/// <summary>
		/// Creates a new shape, with transform parameters as predecessor and child of predecessor.
		/// </summary>
		/// <param name="successorShape"></param>
		/// <param name="predecessor"></param>
		/// <returns>The new Shape</returns>
		public Shape CreateNewShape3(GameObject successorShape, Shape predecessor)
		{
			GameObject newShapeGO = Instantiate(successorShape);
			Shape newShape = newShapeGO.GetComponent<Shape>();
			newShape.gameObject.name = newShape.Symbol;
			newShape.Position = predecessor.Position;
			newShape.Rotation = predecessor.Rotation;
			newShape.Scale = predecessor.Scale;
			newShapeGO.transform.parent = predecessor.transform;
			return newShape;
		}

		public string ListSuccessorsForNotation()
		{
			string notation = "{";
			for (int i = 0; i < succesor.Count; i++)
			{
				if (succesor[i] == null)
				{
					notation += "NULL";
				}
				else
					notation += succesor[i].Get().GetComponent<Shape>().ToString();
				if (i < succesor.Count - 1)
					notation += " | ";
			}
			notation += "}";
			return notation;
		}

		
	}
}