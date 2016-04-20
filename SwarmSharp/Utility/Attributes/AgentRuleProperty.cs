using System;

namespace SwarmSharp
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class AgentRuleProperty : Attribute
	{
		public Type PropertyType { get; set; }
		public AgentRuleProperty (Type propertyType)
		{
			PropertyType = propertyType;
		}
	}
}

