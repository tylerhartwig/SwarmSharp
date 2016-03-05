using System;

namespace SwarmSharp
{
	public struct Vector
	{
		public Vector(int x = 0, int y = 0) {
			this.X = x;
			this.Y = y;
		}

		public int X;
		public int Y;
	}
}

