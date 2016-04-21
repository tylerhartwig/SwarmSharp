using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace SwarmSharp
{
	public class MovementRuleBuilder
	{
		public TypeInfo BuildingRuleType { get; private set; }
		Point ownerPosition;

		static Dictionary<string, TypeInfo> ruleTypes;
		public static Dictionary<string, TypeInfo> RuleTypes {
			get {
				if (ruleTypes == null) {
					ruleTypes = new Dictionary<string, TypeInfo> ();
					var assembly = typeof(MovementRuleBuilder).GetTypeInfo ().Assembly;
					var types = assembly.DefinedTypes;
					foreach (var type in types) {
						var attribute = type.GetCustomAttribute<AgentRuleName> ();
						if (attribute != null)
							ruleTypes.Add (attribute.Name, type);
					}
				}
				return ruleTypes;
			}
		}

		Dictionary<string, Tuple<string, ITargetable>> targets = new Dictionary<string, Tuple<string, ITargetable>> ();
		Dictionary<string, int> targetCount = new Dictionary<string, int> ();

		public string BuildingName {
			get {
				return BuildingRuleType.GetCustomAttribute<AgentRuleName> ().Name;
			}
		}

		public MovementRuleBuilder() { 
			BuildingRuleType = null;
		}

		public MovementRuleBuilder (string ruleName) : this(RuleTypes[ruleName]) { }

		public MovementRuleBuilder (TypeInfo movementRuleType)
		{
			BuildingRuleType = movementRuleType;
			InitializeTargets ();
		}

		public MovementAgentRule Build(){
			if (ownerPosition == null)
				throw new NullReferenceException ("MovementRuleBuilder needs owner's Position to build!");
			var rule = Activator.CreateInstance (BuildingRuleType.AsType ()) as MovementAgentRule;
			rule.SetOwner (ownerPosition);
			var properties = BuildingRuleType.DeclaredProperties.Where(p => p.GetCustomAttribute<AgentRuleTarget> () != null);
			foreach (var target in targets) {
				var property = properties.Where (p => p.GetCustomAttribute<AgentRuleTarget> ().Name == target.Key).First ();
				var points = target.Value.Item2.GetTargets ().ToArray ();
				var random = new Random ();
				if (targetCount [target.Key] == 1) {
					property.SetValue (rule, points [random.Next (points.Count ())]);
				} else if (targetCount [target.Key] == -1) {
					var list = new List<Point> (points);
					property.SetValue (rule, list);
				} else {
					var list = new List<Point> ();
					for (int i = 0; i < targetCount [target.Key]; i++) {
						list.Add (points [random.Next (points.Count())]);
					}
					property.SetValue (rule, list);
				}
			}
			return rule;
		}

		public void ChangeType (string name) {
			ChangeType (RuleTypes [name]);
		}

		public void ChangeType (TypeInfo type) {
			BuildingRuleType = type;
			InitializeTargets ();
		}

		void InitializeTargets(){
			targets.Clear ();
			targetCount.Clear ();
			foreach (var property in BuildingRuleType.DeclaredProperties) {
				AgentRuleTarget attribute = property.GetCustomAttribute<AgentRuleTarget> ();
				if (attribute != null) {
					targets.Add (attribute.Name, null);
					targetCount.Add (attribute.Name, attribute.Count);
				}
			}
		}

		public void SetOwnerPosition(Agent owner){
			ownerPosition = owner.Position;
		}

		public void SetOwnerPosition(Point position){
			ownerPosition = position;
		}

		public void SetTarget(string name, ITargetable target) {
			targets [name] = new Tuple<string, ITargetable> (target.Name, target);
		}

		public IEnumerable<string> GetTargets(){
			return targets.Keys.ToList ();
		}

		public string GetTargetReference(string name){
			if (targets [name] != null)
				return targets [name].Item1;
			else
				return null;
		}
	}
}

