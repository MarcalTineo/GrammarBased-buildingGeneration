using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	public class RuleReplace : Rule
	{
		public override List<Shape> ApplyRule(Shape shape)
		{
			Shape newShape = CreateNewShape3(succesor[0], shape);
			return new List<Shape> { newShape }; 
		}
	}
}

