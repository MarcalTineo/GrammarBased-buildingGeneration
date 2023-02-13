using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Grammar", menuName = "GBBG/Grammar")]
public class Grammar : ScriptableObject
{
    //public Dictionary<string, Shape> shapes;
    public List<Shape> shapesList;
    public List<Rule> rules;


}
