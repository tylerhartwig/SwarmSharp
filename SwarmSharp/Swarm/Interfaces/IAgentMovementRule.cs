using System;
using System.Collections.Generic;

namespace SwarmSharp
{
	public interface IAgentMovementRule
	{
		Step CalculateStep();
		int NumTargets { get; }
		string[] TargetList { get; }
		void SetOwner(Agent owner);
		void SetTarget (string name, Swarm target);
		Swarm GetTarget(string name);
		void RotateTargets(bool allowSelfReference = true);
		IAgentMovementRule Copy();
	}
}

