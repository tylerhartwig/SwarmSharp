using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SwarmSharp
{
	public class Swarm
	{
		public string Name { get; set; }
		public List<Agent> Agents { get; set; } = new List<Agent>();
		public AgentColor Color { get; set; } = AgentColor.Blue;
		public AgentShape Shape { get; set; } = AgentShape.Square;
		public IAgentMovementRule MovementRule { get; set; } 
		public bool SelfReference { get; set; }

		public Swarm () {
			Agents = new List<Agent> ();
		}

		public void Setup(){
			foreach (var agent in Agents) {
				MovementRule.SetOwner (agent);
				MovementRule.RotateTargets (SelfReference);
				agent.MovementRule = MovementRule.Copy();
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

		public void SetCount(int count){
			while (Agents.Count < count) {
				Agents.Add (new Agent ());
			}
			while (Agents.Count > count) {
				Agents.RemoveAt (0);
			}
		}
	}
}

