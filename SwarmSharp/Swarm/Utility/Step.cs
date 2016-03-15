using System;

namespace SwarmSharp
{
	public class Step
	{
		public Vector Direction;
		public double Size = 0.25;
		public double X {
			get {
				return (Direction.X * Size);
			}
		}

		public double Y {
			get {
				return (Direction.Y * Size);
			}
		}

		public Step () {
			Direction = new Vector ();
		}
	}
}

