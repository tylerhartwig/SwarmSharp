using System;
using System.Collections.Generic;
using System.Reflection;

using Xamarin.Forms;
using System.Linq;

namespace SwarmSharp
{
	public class AgentRuleFactory
	{
		// Singleton
		private static AgentRuleFactory instance;
		public static AgentRuleFactory Instance {
			get { 
				if (instance == null)
					instance = new AgentRuleFactory ();
				return instance;
			}
		}

		private Dictionary<string, Type> rules = new Dictionary<string, Type> ();

		private AgentRuleFactory () {
			var assembly = typeof(AgentRuleFactory).GetTypeInfo ().Assembly;
			var types = assembly.DefinedTypes;
			foreach (var type in types) {
				var attribute = type.GetCustomAttributes().Where (a => a is AgentRule).FirstOrDefault ();
				if(attribute != null)
					rules.Add (((AgentRule)attribute).Name, type.AsType ());
			}
		}

		public List<string> GetRuleNames () {
			return rules.Keys.ToList ();
		}

		public IAgentRule CreateRule (string name) {
			var rule = rules [name];
			return Activator.CreateInstance (rule) as IAgentRule;
		}

	}
}

