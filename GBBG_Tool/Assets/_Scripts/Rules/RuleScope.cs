using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	[CreateAssetMenu(fileName = "New Scope Rule", menuName = "GBBG/Rules/Scope")]
	public class RuleScope : Rule
	{
		public Vector3 translatrion;
		public Vector3 rotation;
		[Tooltip("Set to -1 to not affect the size, set to 0 to bring the shape to 2D")]
		public Vector3 scale;

		public override List<Shape> ApplyRule(Shape shape)
		{
			shape.transform.Translate(translatrion);
			shape.transform.Rotate(rotation);
			Vector3 newScale = shape.Scale;
			if (scale.x != -1)
			{
				if (scale.x == 0)
				{
					newScale.x = Shape.disabledDimension;
					if (shape.Scale.x != Shape.disabledDimension)
					{
						shape.Set2D();
					}
				}
				else if (shape.Scale.x == Shape.disabledDimension)
				{
					newScale.x = scale.x;
					shape.Set3D();
				}
				else
					newScale.x = scale.x;
			}
			if (scale.y != -1)
			{
				if (scale.y == 0)
				{
					newScale.y = Shape.disabledDimension;
					if (shape.Scale.y != Shape.disabledDimension)
					{
						shape.Set2D();
					}
				}
				else if (shape.Scale.y == Shape.disabledDimension)
				{
					newScale.y = scale.y;
					shape.Set3D();
				}
				else
					newScale.y = scale.y;
			}
			if (scale.z != -1)
			{
				if (scale.z == 0)
				{
					newScale.z = Shape.disabledDimension;
					if (shape.Scale.z != Shape.disabledDimension)
					{
						shape.Set2D();
					}
				}
				else if (shape.Scale.z == Shape.disabledDimension)
				{
					newScale.z = scale.z;
					shape.Set3D();
				}
				else
					newScale.z = scale.z;
			}
			shape.Scale= newScale;
			return new List<Shape> { shape };
		}
	}
}