using System.Collections.Generic;
using Unity.VisualScripting;
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
			//Debug.Log(GetRuleNotation());
			Shape newShape = CreateNewShape3(succesor[0].Get(), shape);
			if (applyTranslation)
				ApplyTranslation(newShape);
			if (applyRotation)
				ApplyRotation(newShape);
			if (applyScale)
				ApplyScale(newShape);
			return new List<Shape> { newShape };
		}


		private void ApplyTranslation(Shape shape)
		{
			Transform rootParent = GetRootParent(shape.transform);
			switch (translationMode)
			{
				case Mode.Add:
					if (translationSpace == Space.Self)
						shape.transform.Translate(translation, Space.Self);
					else
						shape.transform.Translate(translation, rootParent);
					break;
				case Mode.Set:
					shape.transform.position = rootParent.position + rootParent.rotation * translation;
					break;
				default:
					break;
			}
		}

		private void ApplyRotation(Shape shape)
		{
			Transform rootParent = GetRootParent(shape.transform);
			switch (rotationMode)
			{
				case Mode.Add:
					if (translationSpace == Space.Self)
						shape.transform.Rotate(rotation, Space.Self);
					else
						shape.transform.rotation *= (rootParent.rotation * Quaternion.Euler(rotation));
					break;
				case Mode.Set:
					shape.transform.rotation = rootParent.rotation * Quaternion.Euler(rotation);
					break;
				default:
					break;
			}
		}

		private void ApplyScale(Shape shape)
		{
			Vector3 newScale;
			switch (scaleMode)
			{
				case Mode.Add:
					switch (scaleAbsolute)
					{
						case Mode2.Absolute:
							newScale = shape.Scale + scale;
							break;
						case Mode2.Relative:
							newScale = new Vector3(shape.Scale.x * scale.x, shape.Scale.y * scale.y, shape.Scale.z * scale.z);
							break;
						default:
							newScale = scale;
							Debug.LogError("Scale absolute not set in enum bounds");
							break;
					}

					newScale.x = Mathf.Max(0, newScale.x);
					newScale.y = Mathf.Max(0, newScale.y);
					newScale.z = Mathf.Max(0, newScale.z);

					break;
				case Mode.Set:
					newScale = scale;

					break;
				default:
					newScale = scale;
					Debug.LogError("Scale mode not set in enum bounds");
					break;
			}

			if (newScale.x == 0)
			{
				newScale.x = Shape.disabledDimension;
				if (shape.Scale.x != Shape.disabledDimension)
					shape.Set2D();
			}
			else if (scale.x == -1)
				newScale.x = shape.Scale.x;
			else if (shape.Scale.x == Shape.disabledDimension)
				shape.Set3D();

			if (newScale.y == 0)
			{
				newScale.y = Shape.disabledDimension;
				if (shape.Scale.y != Shape.disabledDimension)
					shape.Set2D();
			}
			else if (scale.y == -1)
				newScale.y = shape.Scale.y;
			else if (shape.Scale.y == Shape.disabledDimension)
				shape.Set3D();

			if (newScale.z == 0)
			{
				newScale.z = Shape.disabledDimension;
				if (shape.Scale.z != Shape.disabledDimension)
					shape.Set2D();
			}
			else if (scale.z == -1)
				newScale.z = shape.Scale.z;
			else if (shape.Scale.z == Shape.disabledDimension)
				shape.Set3D();

			shape.Scale = newScale;	
		}

		/// <summary>
		/// Get the top hierarchy Transform.
		/// </summary>
		/// <param name="tf"></param>
		/// <returns>The root parent transform. Null if this transform is the root</returns>
		private static Transform GetRootParent(Transform tf)
		{
			Transform parent = tf.parent;
			while (parent != null)
			{
				tf = parent; 
				parent = parent.parent;
			}
			return tf;
		}
	}
}