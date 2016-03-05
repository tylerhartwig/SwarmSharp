using System;

namespace SwarmSharp
{
	public struct Point
	{
		public Point() {
			X = 0;
			Y = 0;
		}
		public int X;
		public int Y;

		public static Vector operator-(Point one, Point two) {
			new Vector (one.X - two.X, one.Y - two.Y);
		}
	}
}

