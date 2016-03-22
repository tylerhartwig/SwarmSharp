using System;

namespace SwarmSharp
{
	public class AgentRule : Attribute
	{
		public string Name;

		public AgentRule (string name)
		{
			Name = name;
		}
	}
}

