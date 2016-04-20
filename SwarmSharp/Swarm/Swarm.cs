using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SwarmSharp
{
	public class Swarm : ITargetable
	{
		public string Name { get; set; }
		public int Count { get; set; }
		public List<Agent> Agents { get; set; } = new List<Agent>();
		public AgentColor Color { get; set; } = AgentColor.Blue;
		public AgentShape Shape { get; set; } = AgentShape.Square;
		public MovementRuleBuilder MovementRuleBuilder { get; set; }
		public bool SelfReference { get; set; }

		public Swarm () {
			Agents = new List<Agent> ();
		}

		public void Setup(){
			Agents.Clear ();
			for (int i = 0; i < Count; i++) {
				Agents.Add(new Agent());
			}
			foreach (var agent in Agents) {
				MovementRuleBuilder.SetOwnerPosition (agent);
				agent.MovementRule = MovementRuleBuilder.Build ();
			}
		}

		public void RePosition(int width, int height){
			var random = new Random ();
			foreach (var agent in Agents) {
				agent.Position.X = random.Next () % width;
				agent.Position.Y = random.Next () % height;
			}
		}

		public void CalculateStepParallel(){
			Parallel.ForEach (Agents, (agent) => {
				agent.CalculateStep();
			});
		}

		public void StepParallel(int maxWidth, int maxHeight){
			Parallel.ForEach (Agents, (agent) => {
				agent.TakeStep(maxWidth, maxHeight);
			});
		}

		#region ITargetable implementation

		public IEnumerable<Point> GetTargets ()
		{
			var list = new List<Point> ();
			foreach (var agent in Agents) {
				list.Add (agent.Position);
			}
			return list;
		}

		#endregion
	}
}

