using System;

namespace SwarmSharp
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class AgentRuleProperty : Attribute
	{
		public Type PropertyType { get; set; }
		public AgentRuleProperty (Type propertyType)
		{
			PropertyType = propertyType;
		}
	}
}

