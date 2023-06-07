using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
    [System.Serializable]
    public class SuccessorData
	{
        public float chance;
        public GameObject successor;

        public SuccessorData()
		{
            chance = 1;
            successor = null;
		}

        public SuccessorData(GameObject go)
		{
            chance = 1;
            successor = go;
		}

        public SuccessorData(float value, GameObject gameObject)
		{
            chance = value;
            successor = gameObject;
		}

	}

    [System.Serializable]
    public class Successor
    {
        public List<SuccessorData> possibleSuccessors = new List<SuccessorData>();

        List<SuccessorData> normalizedSuccessors = new List<SuccessorData>();

        public Successor()
        {
            possibleSuccessors = new List<SuccessorData>() { new SuccessorData() };
        }

        public GameObject Get()
		{
            normalizedSuccessors = NormalizeChance(possibleSuccessors);

            float random = UnityEngine.Random.value;

            float current = 0;
			foreach (SuccessorData data in normalizedSuccessors)
			{
                current += data.chance;
                if(current > random)      
                    return data.successor;
			}
            Debug.LogError("Chances not normalized");
            return null;
		}

		private List<SuccessorData> NormalizeChance(List<SuccessorData> possibleSuccessors)
        { 
            float totalValue = 0;
            List<SuccessorData> result = new List<SuccessorData>();
			foreach (SuccessorData data in possibleSuccessors)
				totalValue += data.chance;

			foreach (SuccessorData data in possibleSuccessors)
                result.Add(new SuccessorData(data.chance / totalValue, data.successor));

			return result;
		}

        public List<string> GetVocabulary()
		{
            List<string> result = new List<string>();
			foreach (SuccessorData data in possibleSuccessors)
			{
                if (data.successor == null)
                    continue;
                result.Add(data.successor.GetComponent<Shape>().Symbol);
			}
            return result;
		}

        public List<Shape> GetVocabularyShape()
		{
            List<Shape> result = new List<Shape>();
            foreach (SuccessorData data in possibleSuccessors)
            {
                result.Add(data.successor.GetComponent<Shape>());
            }
            return result;
        }
	}

}

