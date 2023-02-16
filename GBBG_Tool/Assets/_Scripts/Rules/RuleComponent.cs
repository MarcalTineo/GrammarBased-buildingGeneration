using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	[CreateAssetMenu(fileName = "New Component Rule", menuName = "GBBG/Rules/Component")]
	public class RuleComponent : Rule
	{
		public enum SplitMode { AllFaces, Sides, SidesPlusTop }
		public SplitMode splitMode;
		//succesors 0=side 1=top 2=bottom


		public override List<Shape> ApplyRule(Shape shape)
		{
			List<Shape> result = new List<Shape>();
			Shape newShape;

			result = GenerateSideFaces(shape);

			switch (splitMode)
			{
				case SplitMode.AllFaces:
					result.Add(GenerateTop(shape));
					newShape = CreateNewShape2(succesor[2], shape.transform);
					newShape.Position = shape.Position + shape.Scale.z * shape.transform.forward;
					newShape.Rotation = shape.Rotation * Quaternion.AngleAxis(270, shape.transform.right);
					newShape.Scale = new Vector3(shape.Scale.x, shape.Scale.z, Shape.disabledDimension);
					newShape.transform.SetParent(shape.transform);
					break;
				case SplitMode.SidesPlusTop:
					result.Add(GenerateTop(shape));
					break;
				default:
					break;
			}

			return result;
		}

		private Shape GenerateTop(Shape shape)
		{
			Shape newShape = CreateNewShape2(succesor[1], shape.transform);
			newShape.Position = shape.Position + shape.Scale.y * shape.transform.up;
			newShape.Rotation = shape.Rotation * Quaternion.AngleAxis(90, shape.transform.right);
			newShape.Scale = new Vector3(shape.Scale.x, shape.Scale.z, Shape.disabledDimension);
			newShape.transform.SetParent(shape.transform);
			return newShape;
		}

		private List<Shape> GenerateSideFaces(Shape shape)
		{
			List<Shape> result = new List<Shape>();

			Shape newShape = CreateNewShape2(succesor[0], shape.transform);
			newShape.Position = shape.Position;
			newShape.Rotation = shape.Rotation;
			newShape.Scale = new Vector3(shape.Scale.x, shape.Scale.y, Shape.disabledDimension);
			newShape.transform.SetParent(shape.transform);
			result.Add(newShape);

			newShape = CreateNewShape2(succesor[0], shape.transform);
			newShape.Position = shape.Position + shape.Scale.x * shape.transform.right;
			newShape.Rotation = newShape.Rotation * Quaternion.AngleAxis(270, Vector3.up);
			newShape.Scale = new Vector3(shape.Scale.z, shape.Scale.y, Shape.disabledDimension);
			newShape.transform.SetParent(shape.transform);
			result.Add(newShape);

			newShape = CreateNewShape2(succesor[0], shape.transform);
			newShape.Position = shape.Position + shape.Scale.x * shape.transform.right + shape.Scale.z * shape.transform.forward;
			newShape.Rotation = newShape.Rotation * Quaternion.AngleAxis(180, Vector3.up);
			newShape.Scale = new Vector3(shape.Scale.x, shape.Scale.y, Shape.disabledDimension);
			newShape.transform.SetParent(shape.transform);
			result.Add(newShape);

			newShape = CreateNewShape2(succesor[0], shape.transform);
			newShape.Position = shape.Position + shape.Scale.z * shape.transform.forward;
			newShape.Rotation = newShape.Rotation * Quaternion.AngleAxis(90, Vector3.up);
			newShape.Scale = new Vector3(shape.Scale.z, shape.Scale.y, Shape.disabledDimension);
			newShape.transform.SetParent(shape.transform);
			result.Add(newShape);
			return result;
		}
	}
}

