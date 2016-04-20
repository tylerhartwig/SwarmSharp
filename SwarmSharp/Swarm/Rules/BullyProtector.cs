using System;
using System.Collections.Generic;
using System.Linq;

namespace SwarmSharp
{
	[AgentRuleName("Bully Protector")]
	public class BullyProtector : MovementAgentRule
	{
		[AgentRuleTarget("Bully Group")]
		Point BullyPosition { get; set; }

		[AgentRuleTarget("Protector Group")]
		Point ProtectorPosition { get; set; }

		public BullyProtector() { }

		public override Step CalculateStep ()
		{
			var goalPoint = ProtectorPosition + (ProtectorPosition - BullyPosition);
			var step = new Step ();
			step.Direction = goalPoint - ownerPosition;
			step.Direction.Normalize ();
			return step;
		}
	}
}

