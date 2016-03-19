using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;

namespace SwarmSharp
{
	public class Playfield
	{
		public int Width { get; set; }
		public int Height { get; set; }

		public List<Agent[]> Groups = new List<Agent[]>();

		public Playfield () { }

		public void AddGroup(Agent[] group){
			Groups.Add (group);
		}

		public void RePosition (){
			var random = new Random ();
			foreach (var group in Groups) {
				foreach (var agent in group) {
					if (Width != 0)
						agent.Position.X = random.Next () % Width;
					if (Height != 0)
						agent.Position.Y = random.Next () % Height;
				}
			}
		}

		public void Iterate (){
			Parallel.ForEach (Groups, (group) => {
				Parallel.ForEach(group, (agent) => {
					agent.CalculateStep ();
				});
			});
			Parallel.ForEach (Groups, (group) => {
				Parallel.ForEach (group, (agent) => {
					agent.TakeStep (Width, Height);
				});
			});
		}

		public void PrintPositions(){
			int groupCount = 0;
			foreach (var group in Groups) {
				int agentCount = 0;
				foreach (var agent in group) {
					Debug.WriteLine ("{0}-{1}: ({2}, {3})", groupCount, agentCount, agent.Position.X, agent.Position.Y);
					agentCount++;
				}
				groupCount++;
			}
		}
	}
}

