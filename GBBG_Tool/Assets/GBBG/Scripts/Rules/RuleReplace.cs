using System;
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
		public bool flip;
		public Axis axis;

		public override List<Shape> ApplyRule(Shape shape)
		{
			Shape newShape = CreateNewShape3(succesor[0].Get(), shape);
			if (flip)
			{
				Axis currentTopAxis = GetTopAxis(newShape);
				
			}


			newShape.Scale = new Vector3(
				preferedSize.x == 1 ? succesor[0].Get().GetComponent<Shape>().PreferedSize.x : shape.Scale.x,
				preferedSize.y == 1 ? succesor[0].Get().GetComponent<Shape>().PreferedSize.y : shape.Scale.y,
				preferedSize.z == 1 ? succesor[0].Get().GetComponent<Shape>().PreferedSize.z : shape.Scale.z);
			return new List<Shape> { newShape };
		}

		private Axis GetTopAxis(Shape shape)
		{
			float axisX = Vector3.Dot(Vector3.up, shape.transform.right);
			float axisY = Vector3.Dot(Vector3.up, shape.transform.up);
			float axisZ = Vector3.Dot(Vector3.up, shape.transform.forward);

			if (Mathf.Abs(axisX) < Mathf.Abs(axisY))
			{
				if (Mathf.Abs(axisZ) < Mathf.Abs(axisY))
				{
					return Axis.Y;
				}
				else
				{
					return Axis.Z;
				}
			}
			else
			{
				if (Mathf.Abs(axisZ) < Mathf.Abs(axisX))
				{
					return Axis.X;
				}
				else
				{
					return Axis.Z;
				}
			}
		}
	}
}

