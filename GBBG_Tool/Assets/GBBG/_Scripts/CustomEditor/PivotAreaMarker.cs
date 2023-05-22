using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	public class PivotAreaMarker : MonoBehaviour
	{
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, 0.2f);
		}
	}
}

