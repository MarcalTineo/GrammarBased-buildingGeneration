using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rule : ScriptableObject 
{
    public string predescesor;
    public List<GameObject> succesor;

	public virtual List<Shape> ApplyRule(Shape shape)
	{
		return null;
	}
}
