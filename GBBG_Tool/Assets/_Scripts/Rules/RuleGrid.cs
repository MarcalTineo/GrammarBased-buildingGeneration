using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	[CreateAssetMenu(fileName = "New Grid Rule", menuName = "GBBG/Rules/Grid")]
	public class RuleGrid : Rule
	{
		[Tooltip("Axis parallel to the cut")]public Axis axis;
		public override List<Shape> ApplyRule(Shape shape)
		{
			List<Shape> result = new List<Shape>();
			Vector2Int tiles;
			Vector2 tileSize;
			switch (axis)
			{
				case Axis.X:

					tiles = GetTiles(shape, Axis.X);
					tileSize = new Vector2(shape.Scale.x / tiles.x, shape.Scale.y / tiles.y);
					for (int i = 0; i < tiles.x; i++)
					{
						for (int j = 0; j < tiles.y; j++)
						{
							Shape newShape = CreateNewShape2(succesor[0], shape.transform);
							newShape.Position = new Vector3(
								shape.Position.x + tileSize.x * i,
								shape.Position.y + tileSize.y * j,
								shape.Position.z);
							newShape.Scale = new Vector3(tileSize.x, tileSize.y, shape.Scale.z);
							newShape.transform.parent = shape.transform;
							result.Add(newShape);

						}
					}
					break;
				case Axis.Y:
					tiles = GetTiles(shape, Axis.Y);
					tileSize = new Vector2(shape.Scale.x / tiles.x, shape.Scale.z / tiles.y);
					for (int i = 0; i < tiles.x; i++)
					{
						for (int j = 0; j < tiles.y; j++)
						{
							Shape newShape = CreateNewShape2(succesor[0], shape.transform);
							newShape.Position = new Vector3(
								shape.Position.x + tileSize.x * i,
								shape.Position.y,
								shape.Position.z + tileSize.y * j);
							newShape.Scale = new Vector3(tileSize.x, shape.Scale.y, tileSize.y);
							newShape.transform.parent = shape.transform;
							result.Add(newShape);
						}
					}
					break;
				case Axis.Z:
					tiles = GetTiles(shape, Axis.Z);
					tileSize = new Vector2(shape.Scale.z / tiles.x, shape.Scale.y / tiles.y);
					for (int i = 0; i < tiles.x; i++)
					{
						for (int j = 0; j < tiles.y; j++)
						{
							Shape newShape = CreateNewShape2(succesor[0], shape.transform);
							newShape.Position = new Vector3(
								shape.Position.x,
								shape.Position.y + tileSize.y * j,
								shape.Position.z + tileSize.x * i);
							newShape.Scale = new Vector3(shape.Scale.x, tileSize.y, tileSize.x);
							newShape.transform.parent = shape.transform;
							result.Add(newShape);
						}
					}
					break;
				default:
					break;
			}
			return result;
		}

		Vector2Int GetTiles(Shape shape, Axis plane)
		{
			Vector2Int tiles = Vector2Int.zero;
			int tilesX = 0;
			int tilesY = 0;
			switch (plane)
			{
				case Axis.X:
					tilesX = succesor[0].GetComponent<Shape>().PreferedSize.x == 0 ? 1 : Mathf.RoundToInt(shape.Scale.x / succesor[0].GetComponent<Shape>().PreferedSize.x);
					tilesY = succesor[0].GetComponent<Shape>().PreferedSize.y == 0 ? 1 : Mathf.RoundToInt(shape.Scale.y / succesor[0].GetComponent<Shape>().PreferedSize.y);
					tiles = new Vector2Int(tilesX, tilesY);
					break;
				case Axis.Y:
					tilesX = succesor[0].GetComponent<Shape>().PreferedSize.x == 0 ? 1 : Mathf.RoundToInt(shape.Scale.x / succesor[0].GetComponent<Shape>().PreferedSize.x);
					tilesY = succesor[0].GetComponent<Shape>().PreferedSize.y == 0 ? 1 : Mathf.RoundToInt(shape.Scale.z / succesor[0].GetComponent<Shape>().PreferedSize.z);
					tiles = new Vector2Int(tilesX, tilesY);
					break;
				case Axis.Z:
					tilesX = succesor[0].GetComponent<Shape>().PreferedSize.x == 0 ? 1 : Mathf.RoundToInt(shape.Scale.z / succesor[0].GetComponent<Shape>().PreferedSize.z);
					tilesY = succesor[0].GetComponent<Shape>().PreferedSize.y == 0 ? 1 : Mathf.RoundToInt(shape.Scale.y / succesor[0].GetComponent<Shape>().PreferedSize.y);
					tiles = new Vector2Int(tilesX, tilesY);
					break;
				default:
					break;
			}
			return tiles;
		}
	}
}