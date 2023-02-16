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
		[SerializeField] [Tooltip("0 means infinity")]Vector3 preferedSize;

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
			gameObject.name = gameObject.name + "_INACTIVE";
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

		//Operators

	}
}