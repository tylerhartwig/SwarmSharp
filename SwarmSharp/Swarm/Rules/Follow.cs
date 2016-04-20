using System;
using System.Collections.Generic;
using System.Linq;

namespace SwarmSharp
{
	[AgentRuleName("Follow")]
	public class Follow : MovementAgentRule
	{
		[AgentRuleTarget("Follow Group")]
		public Point FollowPosition { get; set; }

		public Follow () { }
	
		public override Step CalculateStep ()
		{
			var step = new Step ();
			step.Direction = FollowPosition - ownerPosition;
			step.Direction.Normalize ();
			return step;
		}
	}
}

