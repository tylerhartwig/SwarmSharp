using System;

namespace SwarmSharp
{
	[AgentRuleName("Random Motion")]
	public class RandomMotion : MovementAgentRule
	{
		public RandomMotion () { }

		public override Step CalculateStep ()
		{
			var random = new Random ();
			var step = new Step ();
			step.Direction.X = (random.Next () % 100) - 50;
			step.Direction.Y = (random.Next () % 100) - 50;
			step.Direction.Normalize ();
			return step;
		}
	}
}

