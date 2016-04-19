using System;
using System.Collections.Generic;
using System.Linq;

namespace SwarmSharp
{
	public class Agent
	{
		public Point Position { get; set; } = new Point ();

		public IAgentMovementRule MovementRule { get; set; }
		private Step nextStep;

		public Agent () { }

		public void TakeStep (int maxWidth, int maxHeight) {
			if (nextStep != null){
				Position += nextStep;
			}
		//	nextStep = null;
			if (Position.X > maxWidth)
				Position.X = maxWidth;
			else if (Position.X < 0)
				Position.X = 0;
			if (Position.Y > maxHeight)
				Position.Y = maxHeight;
			else if (Position.Y < 0)
				Position.Y = 0;
		}

		public void CalculateStep() {
			nextStep = MovementRule.CalculateStep ();
		}
	}
}

