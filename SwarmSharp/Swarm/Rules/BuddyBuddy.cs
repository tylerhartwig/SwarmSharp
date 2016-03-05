using System;

namespace SwarmSharp
{
	public class BuddyBuddy : IAgentRule
	{
		private Agent owner;
		private Agent buddyOne;
		private Agent buddyTwo;

		public BuddyBuddy (Agent owner, Agent BuddyOne, Agent BuddyTwo)
		{
			this.owner = owner;
			this.buddyOne = BuddyOne;
			this.buddyTwo = BuddyTwo;
		}

		#region IAgentRule implementation

		Step IAgentRule.CalculateStep ()
		{
			Point midpoint = PointUtility.Midpoint (buddyOne.Position, buddyTwo.Position);
			Step step = new Step ();
			step.Direction = midpoint - owner.Position;
		}

		#endregion
	}
}

