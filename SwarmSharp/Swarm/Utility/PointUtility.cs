using System;

namespace SwarmSharp
{
	public static class PointUtility
	{
		public static Point Midpoint(Point one, Point two) {
			return new Point((one.X + two.X) / 2, (one.Y + two.Y) / 2);
		}

		public static double Distance(Point one, Point two) {
			return Math.Sqrt(Math.Pow(one.X - two.X, 2.0) + Math.Pow (one.Y - two.Y, 2.0));
		}
	}
}

