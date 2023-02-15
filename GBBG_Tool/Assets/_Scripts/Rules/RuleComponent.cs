using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	public class RuleComponent : Rule
	{
		public enum SplitMode { AllFaces, FourFaces }
		public override List<Shape> ApplyRule(Shape shape)
		{
			return base.ApplyRule(shape);
		}
	}
}

