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
				var attribute = type.GetCustomAttributes().Where (a => a is AgentRuleName).FirstOrDefault ();
				if(attribute != null)
					rules.Add (((AgentRuleName)attribute).Name, type.AsType ());
			}
		}

		public List<string> GetRuleNames () {
			return rules.Keys.ToList ();
		}

		public IAgentMovementRule CreateDefault(){
			return CreateRule (rules.Keys.FirstOrDefault ());
		}

		public IAgentMovementRule CreateRule (string name) {
			var rule = rules [name];
			return Activator.CreateInstance (rule) as IAgentMovementRule;
		}

		public MovementRuleViewModel CreateDefaultViewModel(){
			return CreateRuleViewModel (rules.Keys.FirstOrDefault ());
		}

		public MovementRuleViewModel CreateRuleViewModel(string name){
			return new MovementRuleViewModel () { Model = CreateRule(name) };
		}
	}
}

