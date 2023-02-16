using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace GBBG
{
	[CreateAssetMenu(fileName = "New Split Rule", menuName = "GBBG/Rules/Split")]
	public class RuleSplit : Rule
	{
		
		public Axis plane;
		public enum CuttingType { Relative, FromRoot, ToRoot }
		public CuttingType cuttingType;
		public List<float> splitPoints = new List<float>();


		public override List<Shape> ApplyRule(Shape shape)
		{
			if (splitPoints.Count == succesor.Count)
			{
				List<Shape> result = new List<Shape>();
				float currentPos = 0;
				switch (plane)
				{
					case Axis.X:
						switch (cuttingType)
						{
							case CuttingType.Relative:
								for (int i = 0; i < succesor.Count; i++)
								{
									GameObject newShape = CreateNewShape(succesor[i], shape.transform);
									newShape.transform.position += Vector3.right * currentPos;
									newShape.transform.localScale = new Vector3(
										i == succesor.Count - 1 ? shape.transform.localScale.x - currentPos : shape.transform.localScale.x / (1 / splitPoints[i]),
										shape.transform.localScale.y, shape.transform.localScale.z);
									currentPos += shape.transform.localScale.x / (1 / splitPoints[i]);
									newShape.transform.parent = shape.transform;
									result.Add(newShape.GetComponent<Shape>());
								}
								break;
							case CuttingType.FromRoot:
								for (int i = 0; i < succesor.Count; i++)
								{
									GameObject newShape = CreateNewShape(succesor[i], shape.transform);
									newShape.transform.position += Vector3.right * currentPos;
									newShape.transform.localScale = new Vector3(
										i == succesor.Count - 1 ? shape.transform.localScale.x - currentPos : splitPoints[i],
										shape.transform.localScale.y,
										shape.transform.localScale.z);
									currentPos += splitPoints[i];
									newShape.transform.parent = shape.transform;
									result.Add(newShape.GetComponent<Shape>());
								}
								break;
							case CuttingType.ToRoot:
								currentPos = shape.transform.localScale.x;
								for (int i = 0; i < succesor.Count; i++)
								{
									GameObject newShape = CreateNewShape(succesor[i], shape.transform);
									if (i == succesor.Count - 1)
										newShape.transform.position = shape.transform.position;
									else
									{
										currentPos -= splitPoints[i];
										newShape.transform.position += Vector3.right * currentPos;
									}
									newShape.transform.localScale = new Vector3(
										i == succesor.Count - 1 ? currentPos : splitPoints[i],
										shape.transform.localScale.y,
										shape.transform.localScale.z);
									newShape.transform.parent = shape.transform;
									result.Add(newShape.GetComponent<Shape>());
								}
								break;
							default:
								break;
						}
						break;
					case Axis.Y:
						switch (cuttingType)
						{
							case CuttingType.Relative:
								for (int i = 0; i < succesor.Count; i++)
								{
									GameObject newShape = CreateNewShape(succesor[i], shape.transform);
									newShape.transform.position += Vector3.up * currentPos;
									newShape.transform.localScale = new Vector3(
										shape.transform.localScale.x,
										shape.transform.localScale.y / splitPoints[i],
										shape.transform.localScale.z);
									currentPos += shape.transform.localScale.y / splitPoints[i];
									newShape.transform.parent = shape.transform;
									result.Add(newShape.GetComponent<Shape>());
								}
								break;
							case CuttingType.FromRoot:
								for (int i = 0; i < succesor.Count; i++)
								{
									GameObject newShape = CreateNewShape(succesor[i], shape.transform);
									newShape.transform.position += Vector3.up * currentPos;
									newShape.transform.localScale = new Vector3(
										shape.transform.localScale.x,
										i == succesor.Count - 1 ? shape.transform.localScale.y - currentPos : splitPoints[i],
										shape.transform.localScale.z);
									currentPos += splitPoints[i];
									newShape.transform.parent = shape.transform;
									result.Add(newShape.GetComponent<Shape>());
								}
								break;
							case CuttingType.ToRoot:
								currentPos = shape.transform.localScale.y;
								for (int i = 0; i < succesor.Count; i++)
								{
									GameObject newShape = CreateNewShape(succesor[i], shape.transform);
									if (i == succesor.Count - 1)
										newShape.transform.position = shape.transform.position;
									else
									{
										currentPos -= splitPoints[i];
										newShape.transform.position += Vector3.up * currentPos;
									}
									newShape.transform.localScale = new Vector3(
										shape.transform.localScale.x,
										i == succesor.Count - 1 ? currentPos : splitPoints[i],
										shape.transform.localScale.z);
									newShape.transform.parent = shape.transform;
									result.Add(newShape.GetComponent<Shape>());
								}
								break;
							default:
								break;
						}
						break;
					case Axis.Z:
						switch (cuttingType)
						{
							case CuttingType.Relative:
								for (int i = 0; i < succesor.Count; i++)
								{
									GameObject newShape = CreateNewShape(succesor[i], shape.transform);
									newShape.transform.position += Vector3.forward * currentPos;
									newShape.transform.localScale = new Vector3(
										shape.transform.localScale.x,
										shape.transform.localScale.y,
										shape.transform.localScale.z / splitPoints[i]);
									currentPos += succesor[i].transform.localScale.z / splitPoints[i];
									newShape.transform.parent = shape.transform;
									result.Add(newShape.GetComponent<Shape>());
								}
								break;
							case CuttingType.FromRoot:
								for (int i = 0; i < succesor.Count; i++)
								{
									GameObject newShape = CreateNewShape(succesor[i], shape.transform);
									newShape.transform.position += Vector3.forward * currentPos;
									newShape.transform.localScale = new Vector3(
										shape.transform.localScale.x,
										shape.transform.localScale.y,
										i == succesor.Count - 1 ? shape.transform.localScale.z - currentPos : splitPoints[i]);
									currentPos += splitPoints[i];
									newShape.transform.parent = shape.transform;
									result.Add(newShape.GetComponent<Shape>());
								}
								break;
							case CuttingType.ToRoot:
								currentPos = shape.transform.localScale.z;
								for (int i = 0; i < succesor.Count; i++)
								{
									GameObject newShape = CreateNewShape(succesor[i], shape.transform);
									if (i == succesor.Count - 1)
										newShape.transform.position = shape.transform.position;
									else
									{
										currentPos -= splitPoints[i];
										newShape.transform.position += Vector3.forward * currentPos;
									}
									newShape.transform.localScale = new Vector3(
										shape.transform.localScale.x,
										shape.transform.localScale.y,
										i == succesor.Count - 1 ? currentPos : splitPoints[i]);
									newShape.transform.parent = shape.transform;
									result.Add(newShape.GetComponent<Shape>());
								}
								break;
							default:
								break;
						}
						break;
					default:
						break;
				}
				return result;
			}
			else
				Debug.LogError("Splits and successors donn't match.");
			return base.ApplyRule(shape);
		}
	}
}