using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	[CreateAssetMenu(fileName = "New Replace Rule", menuName = "GBBG/Rules/Replace")]
	public class RuleReplace : Rule
	{
		[Tooltip("0 = ajust to previous shape, 1 = apply prefered size")]
		public Vector3Int preferedSize;

		public override List<Shape> ApplyRule(Shape shape)
		{
			Shape newShape = CreateNewShape3(succesor[0], shape);
			newShape.Scale = new Vector3(
				preferedSize.x == 1 ? succesor[0].GetComponent<Shape>().PreferedSize.x : newShape.Scale.x,
				preferedSize.y == 1 ? succesor[0].GetComponent<Shape>().PreferedSize.y : newShape.Scale.y,
				preferedSize.z == 1 ? succesor[0].GetComponent<Shape>().PreferedSize.z : newShape.Scale.z);
			return new List<Shape> { newShape }; 
		}
	}
}

