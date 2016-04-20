using System;

namespace SwarmSharp
{
	public class AgentRuleName : Attribute
	{
		public string Name { get; set; }

		public AgentRuleName (string name)
		{
			Name = name;
		}
	}
}

