using UnityEngine;

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
    //Transform transform;
    PrimaryShape shape;
    public bool isTerminal;
   
	//Properties
	public Vector3 Position { get => transform.position; set => transform.position = value; }
	public Quaternion Rotation { get => transform.rotation; set => transform.rotation = value; }
    public Vector3 Scale { get => transform.localScale; set => transform.localScale = value;}
    public string Symbol { get => symbol; }
    public bool IsTerminal { get => isTerminal; }

    //Methods

    //Operators

}
