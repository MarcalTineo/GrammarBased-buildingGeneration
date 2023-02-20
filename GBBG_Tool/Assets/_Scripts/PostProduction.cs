using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	[System.Serializable]
	public class PostProcessRule
	{
		public string Symbol;
		public GameObject Prefab;
	}

	public class PostProduction : MonoBehaviour
	{
		public List<PostProcessRule> postProcessRules;
		GameObject building;
		public void PostProduce(List<Shape> derivation)
		{
			if (CheckTerminal(derivation))
			{
				building = new GameObject("Building");
				foreach (Shape shape in derivation)
				{
					TryGetValue(postProcessRules, shape.Symbol, out GameObject prefab);
					if (prefab != null)
						ReplaceModel(shape, prefab);
				}
			}
			else
				Debug.Log("Not all shapes are terminal in derivation");
		}

		private void ReplaceModel(Shape shape, GameObject prefab)
		{
			//if (shape.Dimensions == 2)
			//{
			//	if (shape.Scale.x == Shape.disabledDimension)
			//		shape.Scale = new Vector3(1, shape.Scale.y, shape.Scale.z);
			//	else if (shape.Scale.y == Shape.disabledDimension)
			//		shape.Scale = new Vector3(shape.Scale.x, 1, shape.Scale.z);
			//	else if (shape.Scale.z == Shape.disabledDimension)
			//		shape.Scale = new Vector3(shape.Scale.x, shape.Scale.y, 1);
			//}
			GameObject model = Instantiate(prefab, building.transform);
			model.transform.position = shape.transform.position;
			model.transform.rotation = shape.transform.rotation;
			model.transform.localScale = shape.Scale + Vector3.forward;
			shape.Deactivate();
		}

		private bool CheckTerminal(List<Shape> derivation)
		{
			foreach (Shape shape in derivation)
			{
				if (!shape.IsTerminal)
					return false;
			}
			return true;
		}

		private void TryGetValue(List<PostProcessRule> rules, string symbol, out GameObject prefab)
		{
			prefab = null;
			foreach (PostProcessRule rule in rules)
			{
				if (rule.Symbol == symbol)
				{
					prefab = rule.Prefab; 
					break;
				}
			}
		}

		internal void Reset()
		{
			DestroyImmediate(building);
		}
	}
}

