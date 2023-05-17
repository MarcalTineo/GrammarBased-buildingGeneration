using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	[CreateAssetMenu(fileName = "New Scope Rule", menuName = "GBBG/Rules/Scope")]
	public class RuleScope : Rule
	{
		public enum Mode { Add, Set}
		public enum Mode2 { Absolute, Relative}

		//translate
		public bool applyTranslation;
		public Vector3 translation;
		public Space translationSpace;
		public Mode translationMode;

		//rotation
		public bool applyRotation;
		public Vector3 rotation;
		public Space rotationSpace;
		public Mode rotationMode;

		//scale
		public bool applyScale;
		[Tooltip("Set to -1 to not affect the size, set to 0 to bring the shape to 2D")]
		public Vector3 scale;
		public Mode scaleMode;
		public Mode2 scaleAbsolute;

		public override string GetRuleNotation()
		{
			string notation = predescesor + " -> Scope( T:" + translation.ToString() + ", R:" + rotation.ToString() + "S:" + scale.ToString() + ")";
			return notation;
		}

		public override List<Shape> ApplyRule(Shape shape)
		{
			Debug.Log(GetRuleNotation());
			if (applyTranslation) 
				ApplyTranslation(shape);
			if (applyRotation)
				ApplyRotation(shape);
			if (applyScale)
				ApplyScale(shape);
			return new List<Shape> { shape };
		}

		private void ApplyScale(Shape shape)
		{
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
			shape.Scale = newScale;
		}

		private void ApplyRotation(Shape shape)
		{
			shape.transform.Rotate(rotation);
		}

		private void ApplyTranslation(Shape shape)
		{
			shape.transform.Translate(translation);
		}
	}
}