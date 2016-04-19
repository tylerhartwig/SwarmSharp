using System;
using System.Collections.Generic;
using System.Linq;

namespace SwarmSharp
{
	[AgentRuleName("Bully Protector")]
	public class BullyProtector : AgentRule, IAgentMovementRule
	{
		Agent owner;
		Agent bully;
		Agent protector;

		public BullyProtector(){
			TargetDictionary = new Dictionary<string, Swarm> ();
			foreach (var target in targetList) {
				TargetDictionary.Add (target, null);
			}
		}

		#region IAgentRule implementation

		public int NumTargets { get { return 2; } }

		string[] targetList = new string[] { "Bully Group", "Protector Group" };
		public string[] TargetList { get { return TargetDictionary.Keys.ToArray (); } }
		Dictionary<string, Swarm> TargetDictionary { get; set; }

		public IAgentMovementRule Copy(){
			return new BullyProtector () { owner = this.owner, bully = this.bully, protector = this.protector };
		}

		Step IAgentMovementRule.CalculateStep ()
		{
			var goalPoint = protector.Position + (protector.Position - bully.Position);
			var step = new Step ();
			step.Direction = goalPoint - owner.Position;
			step.Direction.Normalize ();
			return step;
		}

		public void RotateTargets(bool allowSelfReference = true){
			var bullySwarm = TargetDictionary [targetList [0]];
			var protectorSwarm = TargetDictionary [targetList [1]];
			var random = new Random ();
			var randomBully = bullySwarm.Agents.ToArray() [random.Next () % bullySwarm.Agents.Count];
			var randomProtector = protectorSwarm.Agents.ToArray() [random.Next () % protectorSwarm.Agents.Count];
//			if(!allowSelfReference) {
//				while(Object.ReferenceEquals(randomBully, owner)){
//					randomBully = bullySwarm.Agents [random.Next () % bullySwarm.Agents.Count];
//				}
//				while(Object.ReferenceEquals(randomProtector, owner)){
//					randomProtector = protectorSwarm.Agents [random.Next () % protectorSwarm.Agents.Count];
//				}
//			}	
			bully = randomBully;
			protector = randomProtector;
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
	}
}

