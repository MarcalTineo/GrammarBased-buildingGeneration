using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	public enum Axis { X,Y,Z};
	
	//methods

	public static class Utilities
	{
		/// <summary>
		/// Rounds Vector3 to 3 decimals;
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Vector3 RoundVector3(Vector3 value)
		{
			float x = Mathf.Round(value.x * 1000) / 1000;
			float y = Mathf.Round(value.y * 1000) / 1000;
			float z = Mathf.Round(value.z * 1000) / 1000;
			return new Vector3(x, y, z);
		}
	}
}

