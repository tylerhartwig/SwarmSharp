using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SwarmSharp
{
	public class PlayfieldViewModel : ViewModel
	{
		Playfield playfield;

		private bool isPlaying;
		public bool IsPlaying {
			get { return isPlaying; }
			set { SetProperty (ref isPlaying, value); }
		}

		private Command _togglePlay;
		public Command TogglePlay {
			get { return _togglePlay; }
			set { SetProperty (ref _togglePlay, value); }
		}

		public int Width { 
			get { return playfield.Width; } 
			set { playfield.Width = value; }
		}

		public int Height {
			get { return playfield.Height; }
			set { playfield.Height = value; }
		}

		public PlayfieldViewModel () {
			int numAgents = 5000;
			IsPlaying = false;
			playfield = new Playfield ();
			TogglePlay = new Command (async () => await togglePlay());

			var random = new Random ();
			var agents = new List<Agent> ();
			for (int i = 0; i < numAgents; i++) {
				var agent = new Agent ();
				agents.Add (agent);
			}
			var agentArray = agents.ToArray ();
			foreach (var agent in agentArray) {
				var rule = new BullyProtector (agent, agentArray [random.Next () % numAgents], agentArray [random.Next () % numAgents]);
				agent.AddRule (rule);
			}
			playfield.AddGroup (agentArray);

			playfield.RePosition ();
		}

		public void Reset () {
			playfield.RePosition ();
		}

		public async Task PlayAsync() {
			while (IsPlaying) {
				await Task.Run (() => playfield.Iterate ());
			}
		}

		private async Task togglePlay(){
			if (IsPlaying) {
				IsPlaying = false;
			} else {
				IsPlaying = true;
				await PlayAsync ();
			}
		}

		public List<Agent[]> GetAgents () {
			return playfield.Groups;
		}
	}
}

