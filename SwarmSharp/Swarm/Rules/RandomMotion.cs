using System;

namespace SwarmSharp
{
	[AgentRuleName("Random Motion")]
	public class RandomMotion : AgentRule, IAgentMovementRule
	{
		public RandomMotion ()
		{
		}

		#region IAgentMovementRule implementation

		public Step CalculateStep ()
		{
			var random = new Random ();
			var step = new Step ();
			step.Direction.X = (random.Next () % 100) - 50;
			step.Direction.Y = (random.Next () % 100) - 50;
			step.Direction.Normalize ();
			return step;
		}

		public void SetOwner (Agent owner)
		{
		}

		public void SetTarget (string name, Swarm target)
		{
		}

		public Swarm GetTarget (string name)
		{
			return null;
		}

		public void RotateTargets (bool allowSelfReference = true)
		{
		}

		public IAgentMovementRule Copy ()
		{
			return new RandomMotion ();
		}

		public int NumTargets {
			get {
				return 0;
			}
		}

		public string[] TargetList {
			get {
				return new string[] { };
			}
		}

		#endregion
	}
}

