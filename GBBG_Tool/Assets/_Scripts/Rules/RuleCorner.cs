using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	[CreateAssetMenu(fileName = "New Corner Rule", menuName = "GBBG/Rules/Corner")]
	public class RuleCorner : Rule
	{
		//successors 0=corner 1=side 2=center
		public Axis axis;
		[Tooltip("0 = prefered size.")]
		public float margin = 0; //0 means prefered size.
		public enum MarginType { Absolute, Relative };
		public MarginType marginType;
		public enum SplitMode { FacingOut, FacingUp, FacingDown, FacingLeft, FacingRight }
		public bool includeCenterPiece;

		public override List<Shape> ApplyRule(Shape shape)
		{
			List<Shape> result = new List<Shape>();

			//check margin

			if (marginType == MarginType.Relative && margin > 0.5f)
			{
				Debug.LogError("margin is bigger than half the the piece");
				return result;
			}
			Shape newShape;
			switch (axis)
			{
				case Axis.X:
					if ((margin > shape.Scale.y / 2 || margin > shape.Scale.z / 2) && marginType == MarginType.Absolute)
					{
						Debug.LogError("margin is bigger than half the the piece");
						return result;
					}
					if (includeCenterPiece)
						result.Add(GetCenterPiece(Axis.X, shape));

					Vector2 marginAbsolute = marginType == MarginType.Absolute ? new Vector2(margin, margin) : new Vector2(margin * shape.Scale.z, margin * shape.Scale.y);
					for (int i = 0; i < 3; i++)
					{
						for (int j = 0; j < 3; j++)
						{
							if (i == 1 && j == 1) continue; //center
							if (i % 2 == 0 && j % 2 == 0) //corner
							{
								newShape = CreateNewShape3(succesor[0], shape);
								Vector2 positionDelta = new Vector2(i == 2 ? 0 : shape.Scale.z - marginAbsolute.x, j == 2 ? 0 : shape.Scale.y - marginAbsolute.y);
								newShape.Position = newShape.Position + shape.transform.forward * positionDelta.x + shape.transform.up * positionDelta.y;
								newShape.Scale = new Vector3(shape.Scale.x, marginAbsolute.y, marginAbsolute.x);
							}

							else
							{
								newShape = CreateNewShape3(succesor[1], shape);
								newShape.gameObject.name = "SHAPE" + i + j;
								if (i == 1)
								{
									newShape.Scale = new Vector3(shape.Scale.x, marginAbsolute.y, shape.Scale.z - marginAbsolute.x * 2);
									if (j == 0)
										newShape.Position = newShape.Position + shape.transform.forward * marginAbsolute.x;
									if (j == 2)
										newShape.Position = newShape.Position + shape.transform.up * (shape.Scale.y - marginAbsolute.y) + shape.transform.forward * marginAbsolute.x;
								}
								if (j == 1)
								{
									newShape.Scale = new Vector3(shape.Scale.x, shape.Scale.y - marginAbsolute.y * 2, marginAbsolute.x);
									if (i == 0)
										newShape.Position = newShape.Position + shape.transform.up * marginAbsolute.y;
									if (i == 2)
										newShape.Position = newShape.Position + shape.transform.up * marginAbsolute.y + shape.transform.forward * (shape.Scale.z - marginAbsolute.x);

								}

								//if (i == 1 && j == 0)
								//{
								//	newShape.Position = newShape.Position + shape.transform.forward * marginAbsolute.x;
								//	newShape.Scale = new Vector3(shape.Scale.x, marginAbsolute.y, marginAbsolute.x);
								//}
								//if (i == 0 && j == 1)
								//{
								//	newShape.Position = newShape.Position + shape.transform.up * marginAbsolute.y;
								//	newShape.Scale = new Vector3(shape.Scale.x, marginAbsolute.x, marginAbsolute.y);
								//}
								//if (i == 1 && j == 2)
								//{
								//	newShape.Position = newShape.Position + shape.transform.up * marginAbsolute.y + shape.transform.forward * (shape.Scale.z - marginAbsolute.x);
								//	newShape.Scale = new Vector3(shape.Scale.x, marginAbsolute.y, marginAbsolute.x);
								//}
								//if (i == 2 && j == 1)
								//{
								//	newShape.Position = newShape.Position + shape.transform.up * (shape.Scale.y - marginAbsolute.y) + shape.transform.forward * marginAbsolute.x;
								//	newShape.Scale = new Vector3(shape.Scale.x, marginAbsolute.x, marginAbsolute.y);
								//}

							}
							result.Add(newShape);
						}
					}
					break;
				case Axis.Y:
					if (includeCenterPiece)
						result.Add(GetCenterPiece(Axis.Y, shape));
					break;
				case Axis.Z:
					if (includeCenterPiece)
						result.Add(GetCenterPiece(Axis.Z, shape));
					break;
				default:
					break;
			}
			return result;
		}

		private Shape GetCenterPiece(Axis axis, Shape shape)
		{
			Shape newShape = CreateNewShape3(succesor[2], shape);
			switch (axis)
			{
				case Axis.X:
					if (marginType == MarginType.Absolute)
					{
						newShape.Position = newShape.Position + newShape.transform.up * margin + newShape.transform.forward * margin;
						newShape.Scale = new Vector3(shape.Scale.x, shape.Scale.y - margin * 2, shape.Scale.z - margin * 2);
					}
					else
					{
						Vector2 marginAbsolute = new Vector2(margin * newShape.Scale.z, margin * newShape.Scale.y);
						newShape.Position = newShape.Position + newShape.transform.forward * marginAbsolute.x + newShape.transform.up * marginAbsolute.y;
						newShape.Scale = new Vector3(shape.Scale.x, shape.Scale.y - marginAbsolute.y * 2, shape.Scale.z - marginAbsolute.x * 2);
					}
					break;
				case Axis.Y:
					if (marginType == MarginType.Absolute)
					{
						newShape.Position = newShape.Position + newShape.transform.right * margin + newShape.transform.forward * margin;
						newShape.Scale = new Vector3(shape.Scale.x - margin * 2, shape.Scale.y, shape.Scale.z - margin * 2);
					}
					else
					{
						Vector2 marginAbsolute = new Vector2(margin * newShape.Scale.x, margin * newShape.Scale.z);
						newShape.Position = newShape.Position + newShape.transform.right * marginAbsolute.x + newShape.transform.forward * marginAbsolute.y;
						newShape.Scale = new Vector3(shape.Scale.x - marginAbsolute.x * 2, shape.Scale.y, shape.Scale.z - marginAbsolute.y * 2);
					}
					break;
				case Axis.Z:
					if (marginType == MarginType.Absolute)
					{
						newShape.Position = newShape.Position + newShape.transform.up * margin + newShape.transform.right * margin;
						newShape.Scale = new Vector3(shape.Scale.x - margin * 2, shape.Scale.y - margin * 2, shape.Scale.z);
					}
					else
					{
						Vector2 marginAbsolute = new Vector2(margin * newShape.Scale.x, margin * newShape.Scale.y);
						newShape.Position = newShape.Position + newShape.transform.right * marginAbsolute.x + newShape.transform.up * marginAbsolute.y;
						newShape.Scale = new Vector3(shape.Scale.x - marginAbsolute.x * 2, shape.Scale.y - marginAbsolute.y * 2, shape.Scale.z);
					}
					break;
				default:
					break;
			}
			return newShape;
		}
	}
}

