using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;

namespace SwarmSharp
{
	public class Playfield
	{
//		// Singleton
//		private static Playfield instance;
//		public static Playfield Instance {
//			get { 
//				if (instance == null)
//					instance = new Playfield ();
//				return instance;
//			}
//		}

		public int Width { get; set; }
		public int Height { get; set; }

		public List<Swarm> Swarms = new List<Swarm> ();

		public Playfield () { }

		public void Setup(){
			foreach (var swarm in Swarms) {
				swarm.Setup ();
			}
		}

		public void AddSwarm(Swarm swarm){
			Swarms.Add (swarm);
		}

		public void RePosition (){
			if (Width != 0 && Height != 0)
				foreach (var swarm in Swarms) 
					swarm.RePosition (Width, Height);
		}

		public void Iterate (){
			var result = Parallel.ForEach (Swarms, (swarm) => {
				swarm.CalculateStepParallel();
			});
			while (!result.IsCompleted)
				;
			Parallel.ForEach (Swarms, (swarm) => {
				swarm.StepParallel(Width, Height);
			});
		}
	}
}

