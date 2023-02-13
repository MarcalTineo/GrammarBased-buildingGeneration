using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rule : ScriptableObject 
{
    public Shape predescesor;
    public List<Shape> succesor;

	public virtual List<Shape> ApplyRule(Shape shape)
	{
		return succesor;
	}
}
