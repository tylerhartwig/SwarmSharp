using System;
using System.Collections.Generic;
using SkiaSharp;

using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SwarmSharp
{
	public partial class SwarmFieldPage : ContentPage
	{
		Playfield playfield; 
		Button button = new Button(){
			Text = "Click Me!",
			VerticalOptions = LayoutOptions.End
		};
		SkiaView skiaView;

		public SwarmFieldPage ()
		{
			InitializeComponent ();
			skiaView = new SkiaView (render){
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			Content = new StackLayout {
				Children = {
					skiaView,
					button	
				},
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			button.Clicked += async (object sender, EventArgs e) => {
				int iterations = 1000;
				//watch.Restart();
				for(int i = 0; i < iterations; i++){
					playfield.Iterate();
					await Task.Run(() =>
						((ISkiaViewController)skiaView).Redraw());
				}
				Debug.WriteLine("Finished Iteration!");
				//watch.Stop();
				//Debug.WriteLine("Iteration took: {0}", watch.ElapsedTicks);
			};
		}

		async Task IterateAsync() {
			playfield.Iterate ();
			await Task.Run (() => 
				((ISkiaViewController)skiaView).Redraw ());
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			int numAgents = 1000;

			playfield = new Playfield ();
			playfield.Width = (int)skiaView.Width;
			playfield.Height = (int)skiaView.Height;

			Debug.WriteLine ("Width height: {0}, {1}", playfield.Width, playfield.Height);

			var random = new Random ();
			var agents = new List<Agent> ();
			for (int i = 0; i < numAgents; i++) {
				var agent = new Agent ();
				agent.Position.X = random.Next () % playfield.Width;
				agent.Position.Y = random.Next () % playfield.Height;
				agents.Add (agent);
			}
			var agentArray = agents.ToArray ();
			foreach (var agent in agentArray) {
				var rule = new BuddyBuddy (agent, agentArray [random.Next () % numAgents], agentArray [random.Next () % numAgents]);
				agent.AddRule (rule);
			}
			playfield.AddGroup (agentArray);

			//playfield.PrintPositions ();

			//			for (int i = 0; i < iterations; i++) {
			//				playfield.Iterate ();
			//			}

		}

		void render(SKCanvas canvas, int width, int height){
			//watch.Restart ();
			canvas.Clear (SKColors.Green);
			using (var paint = new SKPaint ()) {
				paint.IsAntialias = true;
				paint.Color = new SKColor (0x2c, 0x3e, 0x50);
				paint.StrokeCap = SKStrokeCap.Round;


				if(playfield != null && playfield.Groups != null)
				foreach (var group in playfield.Groups) {
					foreach (var agent in group) {
						using (var path = new SKPath ()) {
							int X = agent.Position.X;
							int Y = agent.Position.Y;
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
			//watch.Stop ();
			//Debug.WriteLine("Drawing took: {0}", watch.ElapsedTicks);
		}
	}
}

