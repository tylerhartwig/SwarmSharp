using System;

namespace SwarmSharp
{
	[AgentRule("Bully Protector")]
	public class BullyProtector : IAgentRule
	{
		Agent owner;
		Agent bully;
		Agent protector;

		public BullyProtector (Agent owner, Agent bully, Agent protector)
		{
			this.owner = owner;
			this.bully = bully;
			this.protector = protector;
		}

		#region IAgentRule implementation

		Step IAgentRule.CalculateStep ()
		{
			var goalPoint = protector.Position + (protector.Position - bully.Position);
			var step = new Step ();
			step.Direction = goalPoint - owner.Position;
			step.Direction.Normalize ();
			return step;
		}

		#endregion
	}
}

