using System;

namespace SwarmSharp
{
	public class AgentRuleName : Attribute
	{
		public string Name;

		public AgentRuleName (string name)
		{
			Name = name;
		}
	}
}

