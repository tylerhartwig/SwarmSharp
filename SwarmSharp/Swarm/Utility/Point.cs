using System;

namespace SwarmSharp
{
	public class Point : IEquatable<Point>
	{
		public Point(double x = 0, double y = 0) {
			this.X = x;
			this.Y = y;
		}

		public double X { get; set; }
		public double Y { get; set; }

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

		#region IEquatable implementation

		public bool Equals (Point other)
		{
			if (PointUtility.Distance(this, other) < 2.0)
				return true;
			else
				return false;
		}

		#endregion
	}
}

