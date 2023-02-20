using GBBG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Scope : MonoBehaviour
{
	//fields
	private Vector3 scale;
	private Scope parent;

	//properties
	public Vector3 Position { get => transform.position; set => transform.position = value; }
	public Quaternion Rotation { get => transform.rotation; set => transform.rotation = value; }
	public Vector3 Scale
	{
		get => scale;
		set
		{
			scale = value;
			if (parent == null)
				transform.localScale = scale;
			else
			{
				Vector3 realScale = new Vector3(scale.x / transform.parent.lossyScale.x, scale.y / transform.parent.lossyScale.y, scale.z / transform.parent.lossyScale.z);
				transform.localScale = realScale;
			}
		}
	}

	public Scope Predecessor
	{
		get => transform.parent.GetComponent<Scope>();
		set
		{
			parent = value;
			transform.parent = value.transform;
		}
	}

	//methods
	public GameObject GetVisualRepresentation()
	{
		return transform.GetChild(0).gameObject;
	}

	public int GetSuccesorCount()
	{
		return transform.childCount - 1;
	}

	public Shape GetCild(int i)
	{
		return transform.GetChild(i+1).GetComponent<Shape>();
	}

	public void Translate(Vector3 translation)
	{
		transform.Translate(translation);
	}
}
