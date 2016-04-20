using System;
using System.Reflection;

namespace SwarmSharp
{
	public abstract class MovementAgentRule
	{
		protected Point ownerPosition;

		public MovementAgentRule () { }

		public void SetOwner(Point position) {
			ownerPosition = position;
		}

		public abstract Step CalculateStep();
	}
}

