using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	//[System.Serializable]
	//[CreateAssetMenu(fileName = "New Shape", menuName = "GBBG/Shape")]
	public class Shape : MonoBehaviour
	{
		public enum PrimaryShape
		{
			Cube,
			Cylinder,
			Quad
		};
		public List<Mesh> meshes;
		public const float disabledDimension = 0.000001f;

		//private variables
		[SerializeField] string symbol;
		[SerializeField] PrimaryShape geometry;
		[SerializeField] bool isTerminal;
		[SerializeField] int dimensions;
		[SerializeField] [Tooltip("0 means infinity")] Vector3 preferedSize;

		//Properties
		public Vector3 Position { get => transform.position; set => transform.position = value; }
		public Quaternion Rotation { get => transform.rotation; set => transform.rotation = value; }
		public Vector3 Scale
		{
			get => transform.lossyScale;
			set
			{
				Transform parent = transform.parent;
				transform.parent = null;
				transform.localScale = /*Utilities.RoundVector3(value)*/value;
				transform.parent = parent;
			}
		}
		public string Symbol { get => symbol; set => symbol = value; }
		public bool IsTerminal { get => isTerminal; }
		public int Dimensions { get => dimensions; }
		public Vector3 PreferedSize { get => preferedSize; set => preferedSize = value; }
		public PrimaryShape Geometry { get => geometry; set => geometry = value; }


		//Methods
		/// <summary>
		/// Deactivates the shape when a rule is applied to it;
		/// </summary>
		public void Deactivate()
		{
			transform.GetChild(0).gameObject.SetActive(false);
			gameObject.name = "#" + gameObject.name;
		}

		/// <summary>
		/// Sets the shape to 2D. The shape must be 3D to work. The plane is set in the XY axis.
		/// </summary>
		public void Set2D()
		{
			if (Dimensions == 3)
			{
				dimensions = 2;
				geometry = PrimaryShape.Quad;
				GameObject renderer = transform.GetChild(0).gameObject;
				renderer.GetComponent<MeshFilter>().mesh = meshes[(int)PrimaryShape.Quad];
				renderer.transform.localScale = new Vector3(1, 1, disabledDimension);
				renderer.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
			}
			else
			{
				Debug.LogError("Trying to set to 2D a 2D shape.");
			}
		}

		/// <summary>
		/// Sets the shape to 3D. The shape must be 2D for it to work. 
		/// </summary>
		public void Set3D()
		{
			
			if (Dimensions == 2)
			{
				dimensions = 3;
				geometry = PrimaryShape.Cube;
				GameObject renderer = transform.GetChild(0).gameObject;
				renderer.GetComponent<MeshFilter>().mesh = meshes[(int)PrimaryShape.Cube];
				renderer.transform.localScale = new Vector3(1, 1, 1);
				renderer.transform.localPosition = new Vector3(0.5f, 0.5f, 0.5f);

			}
			else
			{
				Debug.LogError("Trying to set to 3D a 3D shape.");
			}
		}

		/// <summary>
		/// Rotates all 3 axis arround the root (diagonal rotation). 
		/// </summary>
		public void RotateCorner()
		{
			RotateRoot(Axis.X, 90);
			RotateRoot(Axis.Y, 270);
			RotateRoot(Axis.Z, 90);
			RotateRoot(Axis.X, 270);
		}

		/// <summary>
		/// Rotates the shape arround an axis, mantaing scale on each axis.
		/// </summary>
		/// <param name="axis">The axis to apply the rotation</param>
		/// <param name="rotation">The rotation in degrees (90, 180, 270)</param>
		public void RotateRoot(Axis axis, int rotation)
		{
			
			if (Dimensions == 3)
			{
				switch (axis)
				{
					case Axis.X:
						switch (rotation)
						{
							case 90:
								Position = Position + transform.up * Scale.y;
								Rotate(new Vector3(90, 0, 0), Space.Self);
								Scale = new Vector3(Scale.x, Scale.z, Scale.y);
								break;
							case 180:
								Position = Position + transform.up * Scale.y + transform.forward * Scale.z;
								Rotate(new Vector3(180, 0, 0), Space.Self);
								//Scale = new Vector3(Scale.x, Scale.z, Scale.y);
								break;
							case 270:
								Position = Position + transform.forward * Scale.z;
								Rotate(new Vector3(270, 0, 0), Space.Self);
								Scale = new Vector3(Scale.x, Scale.z, Scale.y);
								break;
							default:
								break;
						}
						break;
					case Axis.Y:
						
						switch (rotation)
						{
							case 90:
								Position = Position + transform.forward * Scale.z;
								Rotate(new Vector3(0, 90, 0), Space.Self);
								Scale = new Vector3(Scale.z, Scale.y, Scale.x);
								break;
							case 180:
								Position = Position + transform.right * Scale.x + transform.forward * Scale.z;
								Rotate(new Vector3(0, 180, 0), Space.Self);
								//Scale = new Vector3(Scale.x, Scale.y, Scale.z);
								break;
							case 270:
								Position = Position + transform.right * Scale.x;
								Rotate(new Vector3(0, 270, 0), Space.Self);
								Scale = new Vector3(Scale.z, Scale.y, Scale.x);

								break;
							default:
								break;
						}
						break;
					case Axis.Z:
						switch (rotation)
						{
							case 90:
								Position = Position + transform.right * Scale.x;
								Rotate(new Vector3(0, 0, 90), Space.Self);
								Scale = new Vector3(Scale.y, Scale.x, Scale.z);
								break;
							case 180:
								Position = Position + transform.up * Scale.y + transform.right * Scale.x;
								Rotate(new Vector3(0, 0, 180), Space.Self);
								//Scale = new Vector3(Scale.x, Scale.z, Scale.y);
								break;
							case 270:
								Position = Position + transform.up * Scale.y;
								Rotate(new Vector3(0, 0, 270), Space.Self);
								Scale = new Vector3(Scale.y, Scale.x, Scale.z);
								break;
							default:
								break;
						}
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Applies transform.Rotate() in worldSpace.
		/// </summary>
		/// <param name="eulerAngle"></param>
		/// <param name="space"></param>
		private void Rotate(Vector3 eulerAngle, Space space)
		{
			Transform parent = transform.parent;
			transform.parent = null;
			transform.Rotate (eulerAngle, space);
			transform.parent = parent;
		}

		public override string ToString()
		{
			return symbol;
		}
	}
}