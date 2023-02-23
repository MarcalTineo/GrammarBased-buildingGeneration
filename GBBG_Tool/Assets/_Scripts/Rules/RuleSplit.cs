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
		public enum CuttingType { FromRoot, ToRoot }
		public CuttingType cuttingType;
		[Tooltip("Each value is the width of the piece, not the total distance to the root.")]
		public List<float> splitPoints = new List<float>();


		public override List<Shape> ApplyRule(Shape shape)
		{
			if (splitPoints.Count == succesor.Count - 1)
			{
				List<Shape> result = new List<Shape>();
				float currentPos = 0;
				switch (plane)
				{
					case Axis.X:
						if (cuttingType == CuttingType.FromRoot)
						{
							for (int i = 0; i < succesor.Count; i++)
							{
								Shape newShape = CreateNewShape3(succesor[i], shape);
								newShape.Position += newShape.transform.right * currentPos;
								if (i == succesor.Count - 1)
									newShape.Scale = new Vector3(shape.Scale.x - currentPos, shape.Scale.y, shape.Scale.z);
								else
								{
									newShape.Scale = new Vector3(splitPoints[i], shape.Scale.y, shape.Scale.z);
									currentPos += splitPoints[i];
								}
								result.Add(newShape);
							}
						}
						else if (cuttingType == CuttingType.ToRoot)
						{
							currentPos = shape.Scale.x;
							for (int i = 0; i < succesor.Count; i++)
							{
								Shape newShape = CreateNewShape3(succesor[i], shape);
								if (i == succesor.Count - 1)
								{
									newShape.Scale = new Vector3(currentPos, newShape.Scale.y, newShape.Scale.z);
								}
								else
								{
									currentPos -= splitPoints[i];
									newShape.Position += newShape.transform.right * currentPos;
									newShape.Scale = new Vector3(splitPoints[i], newShape.Scale.y, newShape.Scale.z);
								}
								result.Add(newShape);
							}
						}
						else
							Debug.LogError("Cutting type not set. Current value is" + (int)cuttingType);
						break;
					case Axis.Y:
						if (cuttingType == CuttingType.FromRoot)
						{
							for (int i = 0; i < succesor.Count; i++)
							{
								Shape newShape = CreateNewShape3(succesor[i], shape);
								newShape.Position += newShape.transform.up * currentPos;
								if (i == succesor.Count - 1)
									newShape.Scale = new Vector3(shape.Scale.x, shape.Scale.y - currentPos, shape.Scale.z);
								else
								{
									newShape.Scale = new Vector3(shape.Scale.x, splitPoints[i], shape.Scale.z);
									currentPos += splitPoints[i];
								}
								result.Add(newShape);
							}
						}
						else if (cuttingType == CuttingType.ToRoot)
						{
							currentPos = shape.Scale.y;
							for (int i = 0; i < succesor.Count; i++)
							{
								Shape newShape = CreateNewShape3(succesor[i], shape);
								if (i == succesor.Count - 1)
								{
									newShape.Scale = new Vector3(newShape.Scale.x, currentPos, newShape.Scale.z);
								}
								else
								{
									currentPos -= splitPoints[i];
									newShape.Position += newShape.transform.up * currentPos;
									newShape.Scale = new Vector3(newShape.Scale.x, splitPoints[i], newShape.Scale.z);
								}
								result.Add(newShape);
							}
						}
						else
							Debug.LogError("Cutting type not set. Current value is" + (int)cuttingType);
						break;
					case Axis.Z:
						if (cuttingType == CuttingType.FromRoot)
						{
							for (int i = 0; i < succesor.Count; i++)
							{
								Shape newShape = CreateNewShape3(succesor[i], shape);
								newShape.Position += newShape.transform.forward * currentPos;
								if (i == succesor.Count - 1)
									newShape.Scale = new Vector3(shape.Scale.x, shape.Scale.y, shape.Scale.z - currentPos);
								else
								{
									newShape.Scale = new Vector3(shape.Scale.x, shape.Scale.y, splitPoints[i]);
									currentPos += splitPoints[i];
								}
								result.Add(newShape);
							}
						}
						else if (cuttingType == CuttingType.ToRoot)
						{
							currentPos = shape.Scale.z;
							for (int i = 0; i < succesor.Count; i++)
							{
								Shape newShape = CreateNewShape3(succesor[i], shape);
								if (i == succesor.Count - 1)
								{
									newShape.Scale = new Vector3(newShape.Scale.x, newShape.Scale.y, currentPos);
								}
								else
								{
									currentPos -= splitPoints[i];
									newShape.Position += newShape.transform.forward * currentPos;
									newShape.Scale = new Vector3(newShape.Scale.x, newShape.Scale.y, splitPoints[i]);
								}
								result.Add(newShape);
							}
						}
						else
							Debug.LogError("Cutting type not set. Current value is" + (int)cuttingType);
						break;
					default:
						break;
				}


				return result;
			}
			else
				Debug.LogError("Splits and successors don't match. There's " + splitPoints.Count + " splitting points and " + succesor.Count + " successors.");
			return base.ApplyRule(shape);
		}
	}
}
//switch (plane)
//{
//	case Axis.X:
//		switch (cuttingType)
//		{
//			case CuttingType.Relative:
//				for (int i = 0; i < succesor.Count; i++)
//				{
//					GameObject newShape = CreateNewShape(succesor[i], shape.transform);
//					newShape.transform.position += Vector3.right * currentPos;
//					newShape.transform.localScale = new Vector3(
//						i == succesor.Count - 1 ? shape.transform.localScale.x - currentPos : shape.transform.localScale.x / (1 / splitPoints[i]),
//						shape.transform.localScale.y, shape.transform.localScale.z);
//					currentPos += shape.transform.localScale.x / (1 / splitPoints[i]);
//					newShape.transform.parent = shape.transform;
//					result.Add(newShape.GetComponent<Shape>());
//				}
//				break;
//			case CuttingType.FromRoot:
//				for (int i = 0; i < succesor.Count; i++)
//				{
//					GameObject newShape = CreateNewShape(succesor[i], shape.transform);
//					newShape.transform.position += Vector3.right * currentPos;
//					newShape.transform.localScale = new Vector3(
//						i == succesor.Count - 1 ? shape.transform.localScale.x - currentPos : splitPoints[i],
//						shape.transform.localScale.y,
//						shape.transform.localScale.z);
//					currentPos += splitPoints[i];
//					newShape.transform.parent = shape.transform;
//					result.Add(newShape.GetComponent<Shape>());
//				}
//				break;
//			case CuttingType.ToRoot:
//				currentPos = shape.transform.localScale.x;
//				for (int i = 0; i < succesor.Count; i++)
//				{
//					GameObject newShape = CreateNewShape(succesor[i], shape.transform);
//					if (i == succesor.Count - 1)
//						newShape.transform.position = shape.transform.position;
//					else
//					{
//						currentPos -= splitPoints[i];
//						newShape.transform.position += Vector3.right * currentPos;
//					}
//					newShape.transform.localScale = new Vector3(
//						i == succesor.Count - 1 ? currentPos : splitPoints[i],
//						shape.transform.localScale.y,
//						shape.transform.localScale.z);
//					newShape.transform.parent = shape.transform;
//					result.Add(newShape.GetComponent<Shape>());
//				}
//				break;
//			default:
//				break;
//		}
//		break;
//	case Axis.Y:
//		switch (cuttingType)
//		{
//			case CuttingType.Relative:
//				for (int i = 0; i < succesor.Count; i++)
//				{
//					GameObject newShape = CreateNewShape(succesor[i], shape.transform);
//					newShape.transform.position += Vector3.up * currentPos;
//					newShape.transform.localScale = new Vector3(
//						shape.transform.localScale.x,
//						shape.transform.localScale.y / splitPoints[i],
//						shape.transform.localScale.z);
//					currentPos += shape.transform.localScale.y / splitPoints[i];
//					newShape.transform.parent = shape.transform;
//					result.Add(newShape.GetComponent<Shape>());
//				}
//				break;
//			case CuttingType.FromRoot:
//				for (int i = 0; i < succesor.Count; i++)
//				{
//					GameObject newShape = CreateNewShape(succesor[i], shape.transform);
//					newShape.transform.position += Vector3.up * currentPos;
//					newShape.transform.localScale = new Vector3(
//						shape.transform.localScale.x,
//						i == succesor.Count - 1 ? shape.transform.localScale.y - currentPos : splitPoints[i],
//						shape.transform.localScale.z);
//					currentPos += splitPoints[i];
//					newShape.transform.parent = shape.transform;
//					result.Add(newShape.GetComponent<Shape>());
//				}
//				break;
//			case CuttingType.ToRoot:
//				currentPos = shape.transform.localScale.y;
//				for (int i = 0; i < succesor.Count; i++)
//				{
//					GameObject newShape = CreateNewShape(succesor[i], shape.transform);
//					if (i == succesor.Count - 1)
//						newShape.transform.position = shape.transform.position;
//					else
//					{
//						currentPos -= splitPoints[i];
//						newShape.transform.position += Vector3.up * currentPos;
//					}
//					newShape.transform.localScale = new Vector3(
//						shape.transform.localScale.x,
//						i == succesor.Count - 1 ? currentPos : splitPoints[i],
//						shape.transform.localScale.z);
//					newShape.transform.parent = shape.transform;
//					result.Add(newShape.GetComponent<Shape>());
//				}
//				break;
//			default:
//				break;
//		}
//		break;
//	case Axis.Z:
//		switch (cuttingType)
//		{
//			case CuttingType.Relative:
//				for (int i = 0; i < succesor.Count; i++)
//				{
//					GameObject newShape = CreateNewShape(succesor[i], shape.transform);
//					newShape.transform.position += Vector3.forward * currentPos;
//					newShape.transform.localScale = new Vector3(
//						shape.transform.localScale.x,
//						shape.transform.localScale.y,
//						shape.transform.localScale.z / splitPoints[i]);
//					currentPos += succesor[i].transform.localScale.z / splitPoints[i];
//					newShape.transform.parent = shape.transform;
//					result.Add(newShape.GetComponent<Shape>());
//				}
//				break;
//			case CuttingType.FromRoot:
//				for (int i = 0; i < succesor.Count; i++)
//				{
//					GameObject newShape = CreateNewShape(succesor[i], shape.transform);
//					newShape.transform.position += Vector3.forward * currentPos;
//					newShape.transform.localScale = new Vector3(
//						shape.transform.localScale.x,
//						shape.transform.localScale.y,
//						i == succesor.Count - 1 ? shape.transform.localScale.z - currentPos : splitPoints[i]);
//					currentPos += splitPoints[i];
//					newShape.transform.parent = shape.transform;
//					result.Add(newShape.GetComponent<Shape>());
//				}
//				break;
//			case CuttingType.ToRoot:
//				currentPos = shape.transform.localScale.z;
//				for (int i = 0; i < succesor.Count; i++)
//				{
//					GameObject newShape = CreateNewShape(succesor[i], shape.transform);
//					if (i == succesor.Count - 1)
//						newShape.transform.position = shape.transform.position;
//					else
//					{
//						currentPos -= splitPoints[i];
//						newShape.transform.position += Vector3.forward * currentPos;
//					}
//					newShape.transform.localScale = new Vector3(
//						shape.transform.localScale.x,
//						shape.transform.localScale.y,
//						i == succesor.Count - 1 ? currentPos : splitPoints[i]);
//					newShape.transform.parent = shape.transform;
//					result.Add(newShape.GetComponent<Shape>());
//				}
//				break;
//			default:
//				break;
//		}
//		break;
//	default:
//		break;
//}