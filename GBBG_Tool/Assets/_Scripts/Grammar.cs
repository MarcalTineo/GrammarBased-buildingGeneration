using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Grammar", menuName = "GBBG/Grammar")]
    public class Grammar : ScriptableObject
    {
        public List<Rule> rules = new List<Rule>();

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
					Debug.Log(successor);
					Debug.Log(successor.GetComponent<Shape>());
					Debug.Log(successor.GetComponent<Shape>().Symbol);


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
	}

    
}