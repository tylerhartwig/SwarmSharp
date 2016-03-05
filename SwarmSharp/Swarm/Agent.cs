using System;
using System.Collections.Generic;
using System.Linq;

namespace SwarmSharp
{
	public class Agent
	{
		public Point Position { get; set; } = new Point ();

		private List<IAgentRule> rules = new List<IAgentRule>();
		private Step nextStep;

		public Agent () { }

		public void AddRule(IAgentRule rule) {
			rules.Add (rule);
		}

		public void TakeStep (int maxWidth, int maxHeight) {
			if(nextStep != null)
				Position += nextStep;
			nextStep = null;
			if (Position.X > maxWidth)
				Position.X = maxWidth;
			else if (Position.X < 0)
				Position.X = 0;
			if (Position.Y > maxHeight)
				Position.Y = maxHeight;
			else if (Position.Y < 0)
				Position.Y = 0;
		}

		public void CalculateStep() {
			var steps = new List<Step> ();
			foreach (var rule in rules) {
				steps.Add (rule.CalculateStep ());
			}
			nextStep = steps.FirstOrDefault ();
		}
	}
}

