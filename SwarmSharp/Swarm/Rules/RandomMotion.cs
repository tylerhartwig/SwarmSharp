using System;

namespace SwarmSharp
{
	public class RandomMotion : AgentRule, IAgentMovementRule
	{
		public RandomMotion ()
		{
		}

		#region IAgentMovementRule implementation

		public Step CalculateStep ()
		{
			var step = new Step ();
		}

		public void SetOwner (Agent owner)
		{
			throw new NotImplementedException ();
		}

		public void SetTarget (string name, Swarm target)
		{
			throw new NotImplementedException ();
		}

		public Swarm GetTarget (string name)
		{
			throw new NotImplementedException ();
		}

		public void RotateTargets (bool allowSelfReference = true)
		{
			throw new NotImplementedException ();
		}

		public IAgentMovementRule Copy ()
		{
			throw new NotImplementedException ();
		}

		public int NumTargets {
			get {
				0;
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

