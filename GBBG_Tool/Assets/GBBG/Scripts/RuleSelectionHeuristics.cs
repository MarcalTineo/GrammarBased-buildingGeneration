using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
    public static class RuleSelectionHeuristics
    {
        public static Rule GetFirst(List<Rule> validRules)
        {
            return validRules[0];
        }

		public static Rule GetRandom(List<Rule> validRules)
		{
			return validRules[Random.Range(0, validRules.Count)];
		}

		
	}

}
