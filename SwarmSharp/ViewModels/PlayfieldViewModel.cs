using System;
using System.Diagnostics;
using System.Collections.Generic;
using SkiaSharp;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SwarmSharp
{
	public class PlayfieldViewModel : ViewModel
	{
		Playfield playfield;

		private Action<SKCanvas, int, int> renderAction;
		public Action<SKCanvas, int, int> RenderAction{
			get { return renderAction; }
			set { SetProperty (ref renderAction, value); }
		}

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
			int numAgents = 10000;
			IsPlaying = false;
			playfield = new Playfield ();
			RenderAction = new Action<SKCanvas, int, int> (render);
			TogglePlay = new Command (async () => await togglePlay());

			var random = new Random ();
			var agents = new List<Agent> ();
			for (int i = 0; i < numAgents; i++) {
				var agent = new Agent ();
				agents.Add (agent);
			}
			var agentArray = agents.ToArray ();
			foreach (var agent in agentArray) {
				var rule = new BuddyBuddy (agent, agentArray [random.Next () % numAgents], agentArray [random.Next () % numAgents]);
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

		void render(SKCanvas canvas, int width, int height){
			canvas.Clear (SKColors.Green);
			using (var paint = new SKPaint ()) {
				paint.IsAntialias = true;
				paint.Color = new SKColor (0x2c, 0x3e, 0x50);
				paint.StrokeCap = SKStrokeCap.Round;

				if (playfield != null && playfield.Groups != null) {
					foreach (var group in playfield.Groups) {
						foreach (var agent in group) {
							using (var path = new SKPath ()) {
								int X = (int)agent.Position.X;
								int Y = (int)agent.Position.Y;
								path.MoveTo (X, Y);
								path.LineTo (X + 5, Y);
								path.LineTo (X + 5, Y + 5);
								path.LineTo (X, Y + 5);
								path.Close ();

								// draw the Xamagon path
								canvas.DrawPath (path, paint);
							}
						}
					}
				}
			}
		}
	}
}

