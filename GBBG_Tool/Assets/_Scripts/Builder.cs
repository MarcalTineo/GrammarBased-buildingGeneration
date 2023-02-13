using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public Grammar grammar;
	

    public List<Shape> derivation;

	private void Start()
	{
		Build();
	}

	public void Build()
    {

		Derivate(derivation[0]);
		
	}

	private void Derivate(Shape shape)
	{
		if (shape.IsTerminal)
		{
			Debug.Log("Terminal");
		}
		else
		{
			//find rules
			List<Rule> validRules = new List<Rule>();
			foreach (Rule rule in grammar.rules)
			{
				if (rule.predescesor.Symbol == shape.Symbol)
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
				shape.gameObject.SetActive(false);
				if (newShapes.Count > 0)
				{
					foreach (Shape newShape in newShapes)
					{
						newShape.transform.parent = shape.transform;
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
