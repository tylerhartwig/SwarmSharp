using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
			set {
				playfield.Width = value; 
				OnPropertyChanged ();
			}
		}

		public int Height {
			get { return playfield.Height; }
			set { 
				playfield.Height = value;
				OnPropertyChanged ();
			}
		}

		ICommand _addSwarm;
		public ICommand AddSwarm { get { return _addSwarm; } set { SetProperty (ref _addSwarm, value); } }

		ICommand _editSwarm;
		public ICommand EditSwarm { get { return _editSwarm; } set { SetProperty (ref _editSwarm, value); } }

		ICommand _setupPlay;
		public ICommand SetupPlay { get { return _setupPlay; } set { SetProperty (ref _setupPlay, value); } }

		ObservableCollection<SwarmViewModel> swarmViewModels;
		public ObservableCollection<SwarmViewModel> SwarmViewModels { get { return swarmViewModels; } set { SetProperty (ref swarmViewModels, value); } }

		SwarmViewModel selectedSwarm;
		public SwarmViewModel SelectedSwarm { get { return selectedSwarm; } set { SetProperty (ref selectedSwarm, value); } }

		public PlayfieldViewModel () {
			IsPlaying = false;
			playfield = DataService.CurrentPlayField;
			SwarmViewModels = new ObservableCollection<SwarmViewModel> ();
			TogglePlay = new Command (async () => await togglePlay());
			AddSwarm = new Command (() => addSwarm ());
			SetupPlay = new Command (() => setupPlay ());
			EditSwarm = new Command<SwarmViewModel> (editSwarm);

			foreach (var swarm in playfield.Swarms) {
				SwarmViewModels.Add (new SwarmViewModel () { Model = swarm });
			}

			playfield.RePosition ();
		}

		public IEnumerable<SwarmViewModel> GetSwarms(){
			return SwarmViewModels;
		}

		void setupPlay(){
			playfield.Setup ();
			playfield.RePosition ();
			var fieldPage = new SwarmFieldPage ();
			fieldPage.BindingContext = this;
			((NavigationPage)Application.Current.MainPage).PushAsync (fieldPage);
		}

		public void Reset () {
			playfield.RePosition ();
		}

		private void addSwarm(){
			var newSwarm = new Swarm () { Name = "Default Name!" };
			playfield.AddSwarm (newSwarm);
			SwarmViewModels.Add (new SwarmViewModel (newSwarm));
		}

		private void editSwarm(SwarmViewModel selectedItem) {
			DataService.CurrentSwarm = SelectedSwarm.Model;
			var editSwarmPage = new SwarmConfigurationPage ();
			//selectedItem = null;

			if (Application.Current.MainPage is NavigationPage)
				((NavigationPage)Application.Current.MainPage).PushAsync (editSwarmPage);
			else
				throw new Exception ("Main page is not Navigaiton page! cannot navigate!");
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
	}
}

