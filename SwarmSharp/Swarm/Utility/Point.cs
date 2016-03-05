using System;

namespace SwarmSharp
{
	public class Point
	{
		public Point(int x = 0, int y = 0) {
			this.X = x;
			this.Y = y;
		}

		public int X { get; set; }
		public int Y { get; set; }

		public static Vector operator-(Point one, Point two) {
			var vector = new Vector (one.X - two.X, one.Y - two.Y);
			vector.Normalize ();
			return vector;
		}

		public static Point operator+(Point point, Step step){
			var newPoint = point;
			newPoint.X += step.X;
			newPoint.Y += step.Y;
			return newPoint;
		}
	}
}

