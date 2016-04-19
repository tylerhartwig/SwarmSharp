using System;
using System.Reflection;

namespace SwarmSharp
{
	public abstract class AgentRule
	{
		public AgentRule () { }

		public string Name {
			get {
				var type = GetType ().GetTypeInfo ();
				return type.GetCustomAttribute<AgentRuleName> ().Name;
			}
		}
	}
}

