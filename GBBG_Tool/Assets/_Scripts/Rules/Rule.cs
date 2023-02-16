using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	[System.Serializable]
	public class Rule : ScriptableObject
	{
		public enum Axis { X, Y, Z };
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
	}
}