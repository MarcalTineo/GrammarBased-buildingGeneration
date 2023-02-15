using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	[CreateAssetMenu(fileName = "New Repeat Rule", menuName = "GBBG/Rules/Repeat")]
	public class RuleRepeat : Rule
	{
		public Plane plane;
		public override List<Shape> ApplyRule(Shape shape)
		{
			List<Shape> result = new List<Shape>();
			Vector2Int tiles;
			Vector2 tileSize;
			switch (plane)
			{
				case Plane.X:
					
					tiles = GetTileSize(shape, Plane.X);
					Debug.Log(tiles);
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
				case Plane.Y:
					tiles = GetTileSize(shape, Plane.Y);
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
				case Plane.Z:
					tiles = GetTileSize(shape, Plane.Z);
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
							newShape.Scale = new Vector3(shape.Scale.x, tileSize.y,  tileSize.x);
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

		Vector2Int GetTileSize(Shape shape, Plane plane)
		{
			Vector2Int tiles = Vector2Int.zero;
			switch (plane)
			{
				case Plane.X:
					tiles = new Vector2Int(
						Mathf.RoundToInt(shape.Scale.x / succesor[0].GetComponent<Shape>().PreferedSize.x),
						Mathf.RoundToInt(shape.Scale.y / succesor[0].GetComponent<Shape>().PreferedSize.y));
					break;
				case Plane.Y:
					tiles = new Vector2Int(
						Mathf.RoundToInt(shape.Scale.x / succesor[0].GetComponent<Shape>().PreferedSize.x),
						Mathf.RoundToInt(shape.Scale.z / succesor[0].GetComponent<Shape>().PreferedSize.z));
					break;
				case Plane.Z:
					tiles = new Vector2Int(
						Mathf.RoundToInt(shape.Scale.z / succesor[0].GetComponent<Shape>().PreferedSize.z),
						Mathf.RoundToInt(shape.Scale.y / succesor[0].GetComponent<Shape>().PreferedSize.y));
					break;
				default:
					break;
			}
			return tiles;
		}
	}
}
