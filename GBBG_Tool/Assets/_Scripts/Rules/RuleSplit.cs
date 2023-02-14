using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Split Rule", menuName = "GBBG/Rules/Split")]
public class RuleSplit : Rule
{
    public enum Plane { X,Y,Z };
    public Plane plane;
    public enum CuttingType { Relative, FromRoot, ToRoot_WIP }
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
				case Plane.X:
					switch (cuttingType)
					{
						case CuttingType.Relative:
							for (int i = 0; i < succesor.Count; i++)
							{
								GameObject newShape = Instantiate(succesor[i], shape.transform.position + Vector3.right * currentPos, shape.transform.rotation);
								newShape.transform.localScale = new Vector3(
									i == succesor.Count - 1 ? shape.transform.localScale.x - currentPos : shape.transform.localScale.x / (1 / splitPoints[i]),
									shape.transform.localScale.y,
									shape.transform.localScale.z);
								currentPos += shape.transform.localScale.x / (1 / splitPoints[i]);
								result.Add(newShape.GetComponent<Shape>());
							}
							break;
						case CuttingType.FromRoot:
							for (int i = 0; i < succesor.Count; i++)
							{
								GameObject newShape = Instantiate(succesor[i], shape.transform.position + Vector3.right * currentPos, shape.transform.rotation);
								newShape.transform.localScale = new Vector3(
									i == succesor.Count - 1 ? shape.transform.localScale.x : splitPoints[i],
									shape.transform.localScale.y,
									shape.transform.localScale.z);
								currentPos += splitPoints[i];
								result.Add(newShape.GetComponent<Shape>());

							}
							break;
						case CuttingType.ToRoot_WIP:
							for (int i = 0; i < succesor.Count; i++)
							{
								currentPos -= splitPoints[i];
								GameObject newShape = Instantiate(succesor[i], shape.transform.position - Vector3.right * (currentPos + shape.transform.localScale.x), shape.transform.rotation);
								newShape.transform.localScale = new Vector3(
									splitPoints[i],
									shape.transform.localScale.y,
									shape.transform.localScale.z);
								result.Add(newShape.GetComponent<Shape>());
							}
							break;
						default:
							break;
					}
					break;
				case Plane.Y:
					switch (cuttingType)
					{
						case CuttingType.Relative:
							for (int i = 0; i < succesor.Count; i++)
							{
								GameObject newShape = Instantiate(succesor[i], shape.transform.position + Vector3.up * currentPos, shape.transform.rotation);
								newShape.transform.localScale = new Vector3(
									shape.transform.localScale.x,
									shape.transform.localScale.y / splitPoints[i],
									shape.transform.localScale.z);
								currentPos += shape.transform.localScale.y / splitPoints[i];
							}
							break;
						case CuttingType.FromRoot:
							for (int i = 0; i < succesor.Count; i++)
							{
								GameObject newShape = Instantiate(succesor[i], shape.transform.position + Vector3.up * currentPos, shape.transform.rotation);
								newShape.transform.localScale = new Vector3(
									shape.transform.localScale.x,
									splitPoints[i],
									shape.transform.localScale.z);
								currentPos += splitPoints[i];
							}
							break;
						case CuttingType.ToRoot_WIP:
							for (int i = 0; i < succesor.Count; i++)
							{
								currentPos -= splitPoints[i];
								GameObject newShape = Instantiate(succesor[i], shape.transform.position - Vector3.up * (currentPos + shape.transform.localScale.y), shape.transform.rotation);
								newShape.transform.localScale = new Vector3(
									shape.transform.localScale.x,
									splitPoints[i],
									shape.transform.localScale.z);
							}
							break;
						default:
							break;
					}
					break;
				case Plane.Z:
					switch (cuttingType)
					{
						case CuttingType.Relative:
							for (int i = 0; i < succesor.Count; i++)
							{
								GameObject newShape = Instantiate(succesor[i], shape.transform.position + Vector3.forward * currentPos, shape.transform.rotation);
								newShape.transform.localScale = new Vector3(
									shape.transform.localScale.x,
									shape.transform.localScale.y,
									shape.transform.localScale.z / splitPoints[i]);
								currentPos += succesor[i].transform.localScale.z / splitPoints[i];
							}
							break;
						case CuttingType.FromRoot:
							for (int i = 0; i < succesor.Count; i++)
							{
								GameObject newShape = Instantiate(succesor[i], shape.transform.position + Vector3.forward * currentPos, shape.transform.rotation);
								newShape.transform.localScale = new Vector3(
									shape.transform.localScale.y,
									shape.transform.localScale.z,
									splitPoints[i]);
								currentPos += splitPoints[i];
							}
							break;
						case CuttingType.ToRoot_WIP:
							for (int i = 0; i < succesor.Count; i++)
							{
								currentPos -= splitPoints[i];
								GameObject newShape = Instantiate(succesor[i], shape.transform.position - Vector3.forward * (currentPos + shape.transform.localScale.z), shape.transform.rotation);
								newShape.transform.localScale = new Vector3(
									shape.transform.localScale.y,
									shape.transform.localScale.z,
									splitPoints[i]);
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
