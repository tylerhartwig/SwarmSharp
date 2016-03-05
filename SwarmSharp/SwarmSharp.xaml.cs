using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace SwarmSharp
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent ();

			MainPage = new ContentPage {
				Content = new StackLayout {
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Label {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						}
					}
				}
			};
		}

		protected override void OnStart ()
		{
			int numAgents = 10;
			int iterations = 100;

			var playfield = new Playfield ();
			playfield.Width = 100;
			playfield.Height = 100;

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

			playfield.PrintPositions ();

			for (int i = 0; i < iterations; i++) {
				playfield.Iterate ();
			}

			playfield.PrintPositions ();
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

