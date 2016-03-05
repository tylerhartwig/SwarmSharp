using System;

namespace SwarmSharp
{
	public class Vector
	{
		public double X { get; set; }
		public double Y { get; set; }

		public Vector(double x = 0, double y = 0) {
			this.X = x;
			this.Y = y;
		}

		public void Normalize(){
			double divisor = Math.Sqrt (Math.Pow (X, 2.0) + Math.Pow (Y, 2.0));
			X /= divisor;
			Y /= divisor;
		}

	}
}

