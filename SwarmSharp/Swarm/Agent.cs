using System;
using System.Collections.Generic;

namespace SwarmSharp
{
	public class Agent
	{
		public Point Position { get; set; }

		private List<IAgentRule> rules;

		public Agent () { }
	}
}

