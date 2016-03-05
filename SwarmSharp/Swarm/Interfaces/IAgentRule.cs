using System;

namespace SwarmSharp
{
	public interface IAgentRule
	{
		Step CalculateStep();
	}
}

