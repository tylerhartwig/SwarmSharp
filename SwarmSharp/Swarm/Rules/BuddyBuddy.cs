using System;
using System.Collections.Generic;
using System.Linq;

namespace SwarmSharp
{
	[AgentRuleName("Buddy Buddy")]
	public class BuddyBuddy : AgentRule, IAgentMovementRule
	{
		private Agent owner;
		private Agent buddyOne;
		private Agent buddyTwo;


		public int NumTargets { get { return 1; } }
		public Dictionary<string, Swarm> TargetDictionary { get; set; }
		public string[] TargetList { get { return TargetDictionary.Keys.ToArray (); } }

		public BuddyBuddy () { 
			TargetDictionary = new Dictionary<string, Swarm> ();
			TargetDictionary.Add ("Buddy Group", null);
		}


		#region IAgentRule implementation

	
		Step IAgentMovementRule.CalculateStep ()
		{
			if (owner == null)
				throw propertyNotSetException ("Owner");
			if (buddyOne == null || buddyTwo == null)
				throw propertyNotSetException ("Targets (" + NumTargets + ")");
			Point midpoint = PointUtility.Midpoint (buddyOne.Position, buddyTwo.Position);
			Step step = new Step ();
			step.Direction = midpoint - owner.Position;
	
			return step;
		}

		public IAgentMovementRule Copy(){
			return new BuddyBuddy () { owner = this.owner, buddyOne = this.buddyOne, buddyTwo = this.buddyTwo };
		}

		public void RotateTargets(bool allowSelfReference = true){
			var swarm = TargetDictionary.Values.FirstOrDefault ();
			var random = new Random ();
			buddyOne = swarm.Agents [random.Next () % swarm.Agents.Count];
			buddyTwo = swarm.Agents [random.Next () % swarm.Agents.Count];
		}
			
		public void SetOwner(Agent owner){
			this.owner = owner;
		}

		public void SetTarget(string name, Swarm target){
			TargetDictionary [name] = target;
		}

		public Swarm GetTarget(string name){
			return TargetDictionary [name];
		}

		#endregion

		private NullReferenceException propertyNotSetException(string name){
			return new NullReferenceException (String.Format("AgentRule \"BuddyBuddy\" must set {0}", name));
		}
	}
}

