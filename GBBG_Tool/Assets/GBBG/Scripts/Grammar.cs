using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GBBG
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Grammar", menuName = "GBBG/Grammar")]
    public class Grammar : ScriptableObject
    {
        public List<Rule> rules = new List<Rule>();

		public List<PostProcess> postProcesses = new List<PostProcess>();

		public List<string> GetVocabulary()
		{
			List<string> shapes = new List<string>();
			shapes.Add("START");

			foreach (Rule rule in rules)
			{
				foreach (GameObject successor in rule.succesor)
				{
					if (successor == null)
						continue;
					//Debug.Log(successor);
					//Debug.Log(successor.GetComponent<Shape>());
					//Debug.Log(successor.GetComponent<Shape>().Symbol);


					string symbol = successor.GetComponent<Shape>().Symbol;
					if(!shapes.Contains(symbol))
						shapes.Add(symbol);
				}
			}

			return shapes;
		}

		public bool IsVocabulary(string symbol)
		{
			return GetVocabulary().Contains(symbol);
		}

		public List<Shape> GetVocabularyShapes()
		{
			List <Shape> shapes = new List<Shape>();
			List <string> addedShapes = new List<string>();
			foreach (Rule rule in rules)
			{
				foreach (GameObject successor in rule.succesor)
				{
					if (successor == null)
						continue;


					string symbol = successor.GetComponent<Shape>().Symbol;
					if (!addedShapes.Contains(symbol))
					{
						shapes.Add(successor.GetComponent<Shape>());
						addedShapes.Add(symbol);
					}
				}
			}
			return shapes;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="symbol"></param>
		/// <returns>True if its terminal, false if its not terminal or the shape doesn't exist.</returns>
		public bool IsTerminal (string symbol)
		{
			foreach (Shape shape in GetVocabularyShapes())
			{
				if (shape.Symbol == symbol)
					return shape.IsTerminal;
			}
			return false;
		}
	}

    
}