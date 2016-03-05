using System;

namespace SwarmSharp
{
	public class Step
	{
		public Vector Direction;
		public int Size = 2;
		public int X {
			get {
				return (int)(Direction.X * Size);
			}
		}

		public int Y {
			get {
				return (int)(Direction.Y * Size);
			}
		}

		public Step () { }
	}
}

