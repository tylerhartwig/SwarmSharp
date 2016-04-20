using System;

namespace SwarmSharp
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class AgentRuleTarget : Attribute
	{
		public string Name { get; set; }
		public int Count { get; set; }

		public AgentRuleTarget (string name, int count = 1)
		{
			Name = name;
			Count = count;
		}
	}
}

