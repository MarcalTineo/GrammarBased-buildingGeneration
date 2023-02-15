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
			Prism3
		};

		//private variables
		[SerializeField] string symbol;
		PrimaryShape shape;
		[SerializeField] bool isTerminal;
		[SerializeField] int dimensions;
		[SerializeField] Vector3 preferedSize;

		//Properties
		public Vector3 Position { get => transform.position; set => transform.position = value; }
		public Quaternion Rotation { get => transform.rotation; set => transform.rotation = value; }
		public Vector3 Scale { get => transform.localScale; set => transform.localScale = value; }
		public string Symbol { get => symbol; }
		public bool IsTerminal { get => isTerminal; }
		public int Dimensions { get => dimensions; set => dimensions = value; }
		public Vector3 PreferedSize { get => preferedSize; set => preferedSize = value; }

		//Methods

		public void Deactivate()
		{
			transform.GetChild(0).gameObject.SetActive(false);
			gameObject.name = gameObject.name + "_INACTIVE";
		}

		//Operators

	}
}