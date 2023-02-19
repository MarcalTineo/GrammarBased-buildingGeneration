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
		[Tooltip("0 = prefered size for corners.")]
		public float margin = 0; //0 means prefered size.
		public enum MarginType { Absolute, Relative };
		public MarginType marginType;
		public enum SplitMode { FacingOut, FacingUp, FacingDown, FacingLeft, FacingRight }
		public bool includeCenterPiece;

		public override List<Shape> ApplyRule(Shape shape)
		{
			List<Shape> result = new List<Shape>();

			bool marginFromCorners = margin == 0;
			Vector3 cornerSize = Vector3.zero;
			if (marginFromCorners)
			{
				Shape cornerSuccessor = succesor[0].GetComponent<Shape>();
				cornerSize = new Vector3(
					cornerSuccessor.PreferedSize.x >= shape.Scale.x / 2 ? cornerSuccessor.PreferedSize.x : shape.Scale.x / 2,
					cornerSuccessor.PreferedSize.y >= shape.Scale.y / 2 ? cornerSuccessor.PreferedSize.y : shape.Scale.y / 2,
					cornerSuccessor.PreferedSize.z >= shape.Scale.z / 2 ? cornerSuccessor.PreferedSize.z : shape.Scale.z / 2);
			}
			Shape newShape;
			Vector2 marginAbs;

			//check relative margin
			if (marginType == MarginType.Relative && margin > 0.5f)
			{
				Debug.LogError("margin is bigger than half the the piece");
				return result;
			}
			switch (axis)
			{
				case Axis.X:

					//check absolute margin 
					if ((margin > shape.Scale.y / 2 || margin > shape.Scale.z / 2) && marginType == MarginType.Absolute)
					{
						Debug.LogError("margin is bigger than half the the piece");
						return result;
					}

					//get absolute margins
					if (marginFromCorners)
						marginAbs = new Vector2(cornerSize.z, cornerSize.y);
					else
						marginAbs = marginType == MarginType.Absolute ? new Vector2(margin, margin) : new Vector2(margin * shape.Scale.z, margin * shape.Scale.y);

					for (int i = 0; i < 3; i++)
					{
						for (int j = 0; j < 3; j++)
						{
							if (i == 1 && j == 1) //center
							{
								if (includeCenterPiece)
									result.Add(GetCenterPiece(Axis.X, shape, marginAbs));
								continue;
							}
							if (i % 2 == 0 && j % 2 == 0) //corner
							{
								newShape = CreateNewShape3(succesor[0], shape);
								Vector2 positionDelta = new Vector2(i == 2 ? 0 : shape.Scale.z - marginAbs.x, j == 2 ? 0 : shape.Scale.y - marginAbs.y);
								newShape.Position = newShape.Position + shape.transform.forward * positionDelta.x + shape.transform.up * positionDelta.y;
								newShape.Scale = new Vector3(shape.Scale.x, marginAbs.y, marginAbs.x);
							}
							else //edges
							{
								newShape = CreateNewShape3(succesor[1], shape);
								newShape.gameObject.name = "SHAPE" + i + j;
								if (i == 1)
								{
									newShape.Scale = new Vector3(shape.Scale.x, marginAbs.y, shape.Scale.z - marginAbs.x * 2);
									if (j == 0)
										newShape.Position = newShape.Position + shape.transform.forward * marginAbs.x;
									if (j == 2)
										newShape.Position = newShape.Position + shape.transform.up * (shape.Scale.y - marginAbs.y) + shape.transform.forward * marginAbs.x;
								}
								if (j == 1)
								{
									newShape.Scale = new Vector3(shape.Scale.x, shape.Scale.y - marginAbs.y * 2, marginAbs.x);
									if (i == 0)
										newShape.Position = newShape.Position + shape.transform.up * marginAbs.y;
									if (i == 2)
										newShape.Position = newShape.Position + shape.transform.up * marginAbs.y + shape.transform.forward * (shape.Scale.z - marginAbs.x);

								}
							}
							result.Add(newShape);
						}
					}
					break;
				case Axis.Y:
					//check absolute margin
					if ((margin > shape.Scale.x / 2 || margin > shape.Scale.z / 2) && marginType == MarginType.Absolute)
					{
						Debug.LogError("margin is bigger than half the the piece");
						return result;
					}

					//get absolute margins
					if (marginFromCorners)
						marginAbs = new Vector2(cornerSize.x, cornerSize.z);
					else
						marginAbs = marginType == MarginType.Absolute ? new Vector2(margin, margin) : new Vector2(margin * shape.Scale.x, margin * shape.Scale.z);

					for (int i = 0; i < 3; i++)
					{
						for (int j = 0; j < 3; j++)
						{
							if (i == 1 && j == 1) //center
							{
								if (includeCenterPiece)
									result.Add(GetCenterPiece(Axis.Y, shape, marginAbs));
								continue;
							}
							if (i % 2 == 0 && j % 2 == 0) //corner
							{
								newShape = CreateNewShape3(succesor[0], shape);
								Vector2 positionDelta = new Vector2(i == 2 ? 0 : shape.Scale.x - marginAbs.x, j == 2 ? 0 : shape.Scale.z - marginAbs.y);
								newShape.Position = newShape.Position + shape.transform.right * positionDelta.x + shape.transform.forward * positionDelta.y;
								newShape.Scale = new Vector3(marginAbs.x, shape.Scale.y, marginAbs.y);
							}
							else //edges
							{
								newShape = CreateNewShape3(succesor[1], shape);
								if (i == 1)
								{
									newShape.Scale = new Vector3(shape.Scale.x - marginAbs.x * 2, shape.Scale.y, marginAbs.y);
									if (j == 0)
										newShape.Position = newShape.Position + shape.transform.right * marginAbs.x;
									if (j == 2)
										newShape.Position = newShape.Position + shape.transform.forward * (shape.Scale.z - marginAbs.y) + shape.transform.right * marginAbs.x;
								}
								if (j == 1)
								{
									newShape.Scale = new Vector3(marginAbs.x, shape.Scale.y, shape.Scale.z - marginAbs.y * 2);
									if (i == 0)
										newShape.Position = newShape.Position + shape.transform.forward * marginAbs.y;
									if (i == 2)
										newShape.Position = newShape.Position + shape.transform.forward * marginAbs.y + shape.transform.right * (shape.Scale.x - marginAbs.x);
								}
							}
							result.Add(newShape);
						}
					}
					break;
				case Axis.Z:
					//check absolute margin
					if ((margin > shape.Scale.x / 2 || margin > shape.Scale.z / 2) && marginType == MarginType.Absolute)
					{
						Debug.LogError("margin is bigger than half the the piece");
						return result;
					}

					//get absolute margins
					if (marginFromCorners)
						marginAbs = new Vector2(cornerSize.x, cornerSize.y);
					else
						marginAbs = marginType == MarginType.Absolute ? new Vector2(margin, margin) : new Vector2(margin * shape.Scale.x, margin * shape.Scale.y);

					for (int i = 0; i < 3; i++)
					{
						for (int j = 0; j < 3; j++)
						{
							if (i == 1 && j == 1) //center
							{
								if (includeCenterPiece)
									result.Add(GetCenterPiece(Axis.Z, shape, marginAbs));
								continue;
							}
							if (i % 2 == 0 && j % 2 == 0) //corner
							{
								newShape = CreateNewShape3(succesor[0], shape);
								Vector2 positionDelta = new Vector2(i == 2 ? 0 : shape.Scale.x - marginAbs.x, j == 2 ? 0 : shape.Scale.y - marginAbs.y);
								newShape.Position = newShape.Position + shape.transform.right * positionDelta.x + shape.transform.up * positionDelta.y;
								newShape.Scale = new Vector3(marginAbs.x, marginAbs.y, shape.Scale.x);
							}
							else //edges
							{
								newShape = CreateNewShape3(succesor[1], shape);
								if (i == 1)
								{
									newShape.Scale = new Vector3(shape.Scale.x - marginAbs.x * 2, marginAbs.y, shape.Scale.z);
									if (j == 0)
										newShape.Position = newShape.Position + shape.transform.right * marginAbs.x;
									if (j == 2)
										newShape.Position = newShape.Position + shape.transform.up * (shape.Scale.y - marginAbs.y) + shape.transform.right * marginAbs.x;
								}
								if (j == 1)
								{
									newShape.Scale = new Vector3(marginAbs.x, shape.Scale.y - marginAbs.y * 2, shape.Scale.z);
									if (i == 0)
										newShape.Position = newShape.Position + shape.transform.up * marginAbs.y;
									if (i == 2)
										newShape.Position = newShape.Position + shape.transform.up * marginAbs.y + shape.transform.right * (shape.Scale.x - marginAbs.x);
								}
							}
							result.Add(newShape);
						}
					}
					break;
				default:
					break;
			}
			return result;
		}

		private Shape GetCenterPiece(Axis axis, Shape shape, Vector2 marginAbs)
		{
			Shape newShape = CreateNewShape3(succesor[2], shape);
			switch (axis)
			{
				case Axis.X:
					newShape.Position = newShape.Position + newShape.transform.up * marginAbs.y + newShape.transform.forward * marginAbs.x;
					newShape.Scale = new Vector3(shape.Scale.x, shape.Scale.y - marginAbs.y * 2, shape.Scale.z - marginAbs.x * 2);
					break;
				case Axis.Y:
					newShape.Position = newShape.Position + newShape.transform.right * marginAbs.x + newShape.transform.forward * marginAbs.y;
					newShape.Scale = new Vector3(shape.Scale.x - marginAbs.x * 2, shape.Scale.y, shape.Scale.z - marginAbs.y * 2);
					break;
				case Axis.Z:
					newShape.Position = newShape.Position + newShape.transform.up * marginAbs.y + newShape.transform.right * marginAbs.x;
					newShape.Scale = new Vector3(shape.Scale.x - marginAbs.x * 2, shape.Scale.y - marginAbs.y * 2, shape.Scale.z);
					break;
				default:
					break;
			}
			return newShape;
		}
	}
}

