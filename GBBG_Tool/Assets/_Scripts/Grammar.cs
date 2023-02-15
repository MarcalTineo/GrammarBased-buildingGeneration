using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Grammar", menuName = "GBBG/Grammar")]
    public class Grammar : ScriptableObject
    {
        //public Dictionary<string, Shape> shapes;
        public List<Rule> rules;


    }
}