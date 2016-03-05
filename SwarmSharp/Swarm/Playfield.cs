using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace SwarmSharp
{
	public class Playfield
	{
		public int Width { get; set; }
		public int Height { get; set; }

		private List<Agent[]> Groups = new List<Agent[]>();

		public Playfield () { }

		public void AddGroup(Agent[] group){
			Groups.Add (group);
		}

		public void Iterate(){
			foreach (var group in Groups) {
				foreach (var agent in group) {
					agent.CalculateStep ();
				}
			}
			foreach (var group in Groups) {
				foreach (var agent in group) {
					agent.TakeStep (Width, Height);
				}
			}
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

