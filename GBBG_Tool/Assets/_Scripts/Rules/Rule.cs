using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	[System.Serializable]
	public class Rule : ScriptableObject
	{
		public enum Plane { X, Y, Z };
		public string predescesor;
		public List<GameObject> succesor;

		public virtual List<Shape> ApplyRule(Shape shape)
		{
			return null;
		}

		public GameObject CreateNewShape(GameObject newShape, Transform parent)
		{
			newShape = Instantiate(newShape);
			newShape.name = newShape.GetComponent<Shape>().Symbol;
			newShape.transform.position= parent.transform.position;
			newShape.transform.rotation= parent.transform.rotation;
			newShape.transform.localScale= parent.transform.localScale;	
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
	}
}