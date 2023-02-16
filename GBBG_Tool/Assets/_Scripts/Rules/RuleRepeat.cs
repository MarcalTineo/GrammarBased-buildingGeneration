using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	[CreateAssetMenu(fileName = "New Repeat Rule", menuName = "GBBG/Rules/Repeat")]
	public class RuleRepeat : Rule
	{
		//XYZ for 3d, XY for 2d
		[Tooltip("Axis perpendicular to the plane division.")] public Axis axis;

		public override List<Shape> ApplyRule(Shape shape)
		{
			List<Shape> result = new List<Shape>();
			if (shape.Dimensions == 3)
			{
				float preferedSize;
				int tiles;
				float tileSize;
				switch (axis)
				{
					case Axis.X:
						preferedSize = succesor[0].GetComponent<Shape>().PreferedSize.x;
						tiles = preferedSize == 0 ? 1 : Mathf.RoundToInt(shape.Scale.x / preferedSize);
						tileSize = shape.Scale.x / tiles;
						for (int i = 0; i < tiles; i++)
						{
							Shape newShape = CreateNewShape3(succesor[0], shape);
							newShape.Position = shape.Position + Vector3.right * tileSize * i;
							newShape.Scale = new Vector3(tileSize, shape.Scale.y, shape.Scale.z);
							result.Add(newShape);
						}
						break;
					case Axis.Y:
						preferedSize = succesor[0].GetComponent<Shape>().PreferedSize.y;
						tiles = preferedSize == 0 ? 1 : Mathf.RoundToInt(shape.Scale.y / preferedSize);
						tileSize = shape.Scale.y / tiles;
						for (int i = 0; i < tiles; i++)
						{
							Shape newShape = CreateNewShape3(succesor[0], shape);
							newShape.Position = shape.Position + Vector3.up * tileSize * i;
							newShape.Scale = new Vector3(shape.Scale.x, tileSize, shape.Scale.z);
							result.Add(newShape);
						}
						break;
					case Axis.Z:
						preferedSize = succesor[0].GetComponent<Shape>().PreferedSize.z;
						tiles = preferedSize == 0 ? 1 : Mathf.RoundToInt(shape.Scale.z / preferedSize);
						tileSize = shape.Scale.z / tiles;
						for (int i = 0; i < tiles; i++)
						{
							Shape newShape = CreateNewShape3(succesor[0], shape);
							newShape.Position = shape.Position + Vector3.forward * tileSize * i;
							newShape.Scale = new Vector3(shape.Scale.x, shape.Scale.y, tileSize);
							result.Add(newShape);
						}
						break;
					default:
						break;
				}
			}
			else if (shape.Dimensions == 2)
			{
				float preferedSize;
				int tiles;
				float tileSize;
				switch (axis)
				{
					case Axis.X:
						preferedSize = succesor[0].GetComponent<Shape>().PreferedSize.x;
						tiles = preferedSize == 0 ? 1 : Mathf.RoundToInt(shape.Scale.x / preferedSize);
						tileSize = shape.Scale.x / tiles;
						for (int i = 0; i < tiles; i++)
						{
							Shape newShape = CreateNewShape3(succesor[0], shape);
							newShape.Position = shape.Position + Vector3.right * tileSize * i;
							newShape.Scale = new Vector3(tileSize, shape.Scale.y, Shape.disabledDimension);
							result.Add(newShape);
						}
						break;
					case Axis.Y:
						preferedSize = succesor[0].GetComponent<Shape>().PreferedSize.y;
						tiles = preferedSize == 0 ? 1 : Mathf.RoundToInt(shape.Scale.y / preferedSize);
						tileSize = shape.Scale.y / tiles;
						for (int i = 0; i < tiles; i++)
						{
							Shape newShape = CreateNewShape3(succesor[0], shape);
							newShape.Position = shape.Position + Vector3.up * tileSize * i;
							newShape.Scale = new Vector3(shape.Scale.x, tileSize, Shape.disabledDimension);
							result.Add(newShape);
						}
						break;
					default:
						break;
				}
			}
			else
				Debug.LogError("Wrong dimensions on shape" + shape.Symbol);
			return result;
		}


	}
}
