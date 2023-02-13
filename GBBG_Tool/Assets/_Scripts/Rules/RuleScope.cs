using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scope Rule", menuName = "GBBG/Rules/Scope")]
public class RuleScope : Rule
{
	public Vector3 translatrion;
	public Vector3 rotation;
	public Vector3 scale;

	public override List<Shape> ApplyRule(Shape shape)
	{
		shape.transform.Translate(translatrion);
		shape.transform.Rotate(rotation);
		shape.transform.localScale = scale;
		return new List<Shape> { shape };
	}
}
