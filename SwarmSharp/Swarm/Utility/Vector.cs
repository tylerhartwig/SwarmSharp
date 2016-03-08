using System;

namespace SwarmSharp
{
	public class Vector
	{
		private double x;
		public double X { 
			get {
				if (Double.IsNaN (x))
					return 0;
				else
					return x;
			}
			set {
				x = value;
			}
		}
		private double y;
		public double Y { 
			get{
				if (Double.IsNaN (y))
					return 0;
				else
					return y;
			}
			set{
				y = value;
			}
		}

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

