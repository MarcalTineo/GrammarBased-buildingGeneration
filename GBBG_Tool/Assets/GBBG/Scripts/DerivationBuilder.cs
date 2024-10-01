using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GBBG
{
	public class DerivationBuilder : ScriptableObject
	{
		static DerivationBuilder instance;
		public static DerivationBuilder GetInstance()
		{
			if (instance == null)
			{
				instance = CreateInstance<DerivationBuilder>();
			}
			return instance;
		}

		List<Shape> axiom;
		List<Shape> derivation;
		Grammar grammar;

		public delegate Rule SelectRuleDelegate(List<Rule> rules);
		public SelectRuleDelegate selectRuleDelegate = RuleSelectionHeuristics.GetFirst;

		GameObject building;
		GameObject derivationRoot;




		public void Build(List<Shape> axiom, Grammar grammar, bool doPostProduction)
		{
			this.axiom = axiom;
			derivation = new List<Shape>();
			foreach (Shape s in axiom)
				derivation.Add(Instantiate(s));
			this.grammar = grammar;

			//set hierarchy
			SetHierarchy();

			//derivation
			int axiomCount = derivation.Count;
			for (int i = 0; i < axiomCount; i++)
			{
				Derivate(derivation[i]);
			}

			if (doPostProduction)
			{
				PostProduce(derivation);
			}
		}

		#region derivation
		private void SetHierarchy()
		{
			derivationRoot = new GameObject("Derivation");

			Vector3 averagePosition = Vector3.zero;
			foreach (Shape shape in derivation)
				averagePosition += shape.transform.position;
			averagePosition /= derivation.Count;
			averagePosition = new Vector3(Mathf.Round(averagePosition.x), Mathf.Round(averagePosition.y), Mathf.Round(averagePosition.z));

			derivationRoot.transform.position = averagePosition;
			foreach (Shape shape in derivation)
			{
				shape.transform.parent = derivationRoot.transform;
				shape.gameObject.SetActive(true);
			}
		}

		private void Derivate(Shape shape)
		{
			if (shape.IsTerminal)
			{
				return;
			}

			//find rules
			List<Rule> validRules = FindRules(shape);

			Rule selectedRule;
			if (validRules.Count <= 0)
			{
				Debug.LogError("No rule for non-terminal shape: " + shape.Symbol + ".");
				return;
			}

			//select rule -- Need to chenge heuristics
			selectedRule = selectRuleDelegate(validRules);
			
			//apply rule
			List<Shape> newShapes = selectedRule.ApplyRule(shape);

			//remove old shape from the derivastion
			derivation.Remove(shape);
			shape.Deactivate();

			//derivate new shapes
			if (newShapes.Count > 0)
			{
				foreach (Shape newShape in newShapes)
				{
					derivation.Add(newShape);
					Derivate(newShape);
				}
			}
		}

		private List<Rule> FindRules(Shape shape)
		{
			List<Rule> validRules = new List<Rule>();
			foreach (Rule rule in grammar.rules)
			{
				if (rule.predescesor == shape.Symbol)
				{
					validRules.Add(rule);
				}
			}
			return validRules;
		}
		#endregion

		#region PostProduction

		public void PostProduce(List<Shape> derivation)
		{
			if (!CheckTerminal(derivation))
			{
				Debug.Log("Not all shapes are terminal in derivation");
				return;
			}

			building = new GameObject("Building");
			foreach (Shape shape in derivation)
			{
				GameObject prefab = TryGetPrefab(grammar.postProcesses, shape.Symbol);
				if (prefab != null)
					ReplaceModel(shape, prefab);
			}
		}

		private void ReplaceModel(Shape shape, GameObject prefab)
		{
			GameObject model = Instantiate(prefab, building.transform);
			model.transform.position = shape.transform.position;
			model.transform.rotation = shape.transform.rotation;
			if (shape.Dimensions == 2)
			{
				model.transform.localScale = new Vector3(shape.Scale.x, shape.Scale.y, 1);
			}
			else
				model.transform.localScale = shape.Scale;
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

		private GameObject TryGetPrefab(List<PostProcess> rules, string symbol)
		{
			GameObject prefab = null;
			foreach (PostProcess rule in rules)
			{
				if (rule.symbol == symbol)
				{
					prefab = rule.asset;
					break;
				}
			}
			return prefab;
		}
		#endregion
	}
}

