using System;
using System.Collections.Generic;
using System.Linq;

namespace SwarmSharp
{
	[AgentRuleName("Buddy Buddy")]
	public class BuddyBuddy : MovementAgentRule
	{
		[AgentRuleTarget("Buddy Group", 2)]
		public List<Point> BuddyGroup { get; set; }

		public BuddyBuddy () { }
	
		public override Step CalculateStep ()
		{
			Point midpoint = PointUtility.Midpoint (BuddyGroup [0], BuddyGroup [1]);
			Step step = new Step ();
			step.Direction = midpoint - ownerPosition;
	
			return step;
		}
	}
}

