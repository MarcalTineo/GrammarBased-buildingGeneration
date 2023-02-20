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
		[SerializeField][Tooltip("0 means infinity")] Vector3 preferedSize;

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
				transform.localScale = value;
				transform.parent = parent;
			}
		}
		public string Symbol { get => symbol; }
		public bool IsTerminal { get => isTerminal; }
		public int Dimensions { get => dimensions; set => dimensions = value; }
		public Vector3 PreferedSize { get => preferedSize; set => preferedSize = value; }
		public PrimaryShape Geometry { get => geometry; set => geometry = value; }

		//Methods

		public void Deactivate()
		{
			transform.GetChild(0).gameObject.SetActive(false);
			gameObject.name = "#" + gameObject.name;
			//transform.localScale = Vector3.one;
		}

		public void Set2D()
		{
			if (dimensions == 3)
			{
				dimensions = 2;
				geometry = PrimaryShape.Quad;
				transform.GetChild(0).GetComponent<MeshFilter>().mesh = meshes[(int)PrimaryShape.Quad];
			}
		}

		public void Set3D()
		{
			if (dimensions == 2)
			{
				dimensions = 3;
				geometry = PrimaryShape.Cube;
				transform.GetChild(0).GetComponent<MeshFilter>().mesh = meshes[(int)PrimaryShape.Cube];
			}
		}

		public void RotateRoot(Rule.Axis axis, int rotation)
		{
			if(dimensions == 3)
			{
				switch (axis)
				{
					case Rule.Axis.X:
						switch (rotation)
						{
							case 90:
								Position = Position + transform.up * Scale.y;
								transform.Rotate(new Vector3(90, 0, 0), Space.Self);
								Scale = new Vector3(Scale.x, Scale.z, Scale.y);
								break;
							case 180:
								Position = Position + transform.up * Scale.y + transform.forward * Scale.z;
								transform.Rotate(new Vector3(180, 0, 0), Space.Self);
								//Scale = new Vector3(Scale.x, Scale.z, Scale.y);
								break;
							case 270:
								Position = Position + transform.forward * Scale.z;
								transform.Rotate(new Vector3(270, 0, 0), Space.Self);
								Scale = new Vector3(Scale.x, Scale.z, Scale.y);
								break;
							default:
								break;
						}
						break;
					case Rule.Axis.Y:
						switch (rotation)
						{
							case 90:
								Position = Position + transform.forward * Scale.z;
								transform.Rotate(new Vector3(0, 90, 0), Space.Self);
								Scale = new Vector3(Scale.z, Scale.y, Scale.x);
								break;
							case 180:
								Position = Position + transform.right * Scale.x + transform.forward * Scale.z;
								transform.Rotate(new Vector3(0, 180, 0), Space.Self);
								//Scale = new Vector3(Scale.x, Scale.z, Scale.y);
								break;
							case 270:
								Position = Position + transform.right * Scale.x;
								transform.Rotate(new Vector3(0, 270, 0), Space.Self);
								Scale = new Vector3(Scale.z, Scale.y, Scale.x);
								break;
							default:
								break;
						}
						break;
					case Rule.Axis.Z:
						switch (rotation)
						{
							case 90:
								Position = Position + transform.right * Scale.x;
								transform.Rotate(new Vector3(0, 0, 90), Space.Self);
								Scale = new Vector3(Scale.y, Scale.x, Scale.z);
								break;
							case 180:
								Position = Position + transform.up * Scale.y + transform.right * Scale.x;
								transform.Rotate(new Vector3(0, 0, 180), Space.Self);
								//Scale = new Vector3(Scale.x, Scale.z, Scale.y);
								break;
							case 270:
								Position = Position + transform.up * Scale.y;
								transform.Rotate(new Vector3(0, 0, 270), Space.Self);
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



		//Operators
		
	}
}