using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GBBG
{
	[RequireComponent(typeof(PostProduction))]
	public class Builder : MonoBehaviour
	{
		[Range(1, 100)] public float sizeX;
		[Range(1, 100)] public float sizeY;
		[Range(1, 100)] public float sizeZ;

		public PostProduction postProduction;

		public Grammar grammar;

		public GameObject axiom;
		public List<Shape> derivation;
		GameObject root;

		private void Start()
		{
			///Build();
		}

		public void Build()
		{
			Derivate(derivation[0]);

			//get the final result
			postProduction.PostProduce(derivation);
		}
		internal void Reset()
		{
			if (derivation.Count > 0)
				DestroyImmediate(root);

			derivation.Clear();
			root = Instantiate(axiom, Vector3.zero, Quaternion.identity);
			root.transform.localScale = new Vector3(sizeX, sizeY, sizeZ);
			root.name = "Start";
			derivation.Add(root.GetComponent<Shape>());
			postProduction.Reset();
		}


		private void Derivate(Shape shape)
		{
			if (shape.IsTerminal)
			{
				//shape.Scale = new Vector3(shape.Scale.x, shape.Scale.y, 1);
				//Debug.Log("Terminal");
			}
			else
			{
				//find rules
				List<Rule> validRules = new List<Rule>();
				foreach (Rule rule in grammar.rules)
				{
					if (rule.predescesor == shape.Symbol)
					{
						validRules.Add(rule);
					}
				}
				//select rule
				Rule selectedRule;
				if (validRules.Count > 0)
				{
					selectedRule = validRules[0/*Random.Range(0, validRules.Count - 1)*/];
					List<Shape> newShapes = selectedRule.ApplyRule(shape);
					derivation.Remove(shape);
					shape.Deactivate();
					if (newShapes.Count > 0)
					{
						foreach (Shape newShape in newShapes)
						{
							derivation.Add(newShape);
							Derivate(newShape);
						}
					}
				}
				else
					Debug.LogError("No rule for non-terminal shape: " + shape.Symbol + ".");
			}
		}
	}
}

