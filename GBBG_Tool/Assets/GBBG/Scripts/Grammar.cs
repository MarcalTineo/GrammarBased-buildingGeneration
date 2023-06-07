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
				foreach (Successor successor in rule.succesor)
				{
					if (successor == null)
						continue;
					List<string> symbols = successor.GetVocabulary();
					foreach (string sym in symbols)
					{
						if (!shapes.Contains(sym))
							shapes.Add(sym);
					}
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
				foreach (Successor successor in rule.succesor)
				{
					if (successor == null)
						continue;

					List<Shape> newShapes = successor.GetVocabularyShape();
					foreach (Shape shape in newShapes)
					{
						string symbol = shape.Symbol;
						if (!addedShapes.Contains(symbol))
						{
							shapes.Add(shape);
							addedShapes.Add(symbol);
						}
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