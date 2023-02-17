using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBBG
{
	public class RuleCorner : Rule
	{
		public Axis axis;
		public float margin; //0 means prefered size.
		public enum MarginType { Absolute, Relative };
		public MarginType marginType;
		public enum SplitMode { FacingOut, FacingUp, FacingDown, FacingLeft, FacingRight}
		public bool includeCenterPiece;

		public override List<Shape> ApplyRule(Shape shape)
		{
			List<Shape> result = new List<Shape>();
			switch (axis)
			{
				case Axis.X:
					if (includeCenterPiece)
						result.Add(GetCenterPiece(Axis.X));
					break;
				case Axis.Y:
					if (includeCenterPiece)
						result.Add(GetCenterPiece(Axis.Y));
					break;
				case Axis.Z:
					if (includeCenterPiece)
						result.Add(GetCenterPiece(Axis.Z));
					break;
				default:
					break;
			}
			return result;
		}

		private Shape GetCenterPiece(Axis x)
		{
			throw new NotImplementedException();
		}
	}
}

