using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Diagnostics;
using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace SwarmSharp
{
	public class SwarmViewModel : ViewModel
	{
		Swarm swarm;
		public Swarm Model { get { return swarm; } set { SetProperty (ref swarm, value); } }

		public string Name {
			get { return swarm.Name; } 
			set { 
				swarm.Name = value;
				OnPropertyChanged ();
			}
		}

		public int Count {
			get { return swarm.Count; }
			set { 
				swarm.Count = value;
				OnPropertyChanged ();
			}
		}

		public bool SelfReference {
			get { return swarm.SelfReference; }
			set {
				swarm.SelfReference = value;
				OnPropertyChanged ();
			}
		}

//		public AgentColor Color {
//			get { return swarm.Color; } 
//			set {
//				swarm.Color = value;
//				OnPropertyChanged ();
//			}
//		}
//
//		public AgentShape Shape {
//			get { return swarm.Shape; } 
//			set {
//				swarm.Shape = value;
//				OnPropertyChanged ();
//			}
//		}

		ICommand _editSwarm;
		public ICommand EditSwarm { get { return _editSwarm; } set { SetProperty (ref _editSwarm, value); } }

		MovementRuleBuilderViewModel movementRule;
		public MovementRuleBuilderViewModel MovementRule { get { return movementRule; }
			set { SetProperty (ref movementRule, value); } }

		int colorIndex;
		public int ColorIndex { get { return colorIndex; } set { SetProperty (ref colorIndex, value); } }

		ObservableCollection<string> agentColors;
		public ObservableCollection<string> AgentColors { get { return agentColors; } 
			set { SetProperty (ref agentColors, value); } }

		int shapeIndex;
		public int ShapeIndex { get { return shapeIndex; } set { SetProperty (ref shapeIndex, value); } }

		ObservableCollection<string> agentShapes;
		public ObservableCollection<string> AgentShapes { get { return agentShapes; } 
			set { SetProperty (ref agentShapes, value); } }

		public SwarmViewModel() : this(new Swarm()) { }
				
		public SwarmViewModel (Swarm model){
			Model = model;			
			MovementRule = new MovementRuleBuilderViewModel (Model.MovementRuleBuilder);
			Initialize ();
		}

		void Initialize(){
			EditSwarm = new Command (editSwarm);
			AgentShapes = new ObservableCollection<string> ();
			AgentColors = new ObservableCollection<string> ();

			foreach (var val in Enum.GetValues (typeof(AgentColor)).Cast<AgentColor> ().ToList ()) {
				AgentColors.Add (val.ToString ());
			}
			foreach (var val in Enum.GetValues (typeof(AgentShape)).Cast<AgentShape> ().ToList ()) {
				AgentShapes.Add (val.ToString ());
			}

			var random = new Random ();
			ColorIndex = random.Next (AgentColors.Count);
			ShapeIndex = random.Next (AgentShapes.Count);			
		}

		void editSwarm () {
			var page = new SwarmConfigurationPage ();
			page.BindingContext = this;
			var mainPage = Application.Current.MainPage;

			if (mainPage is NavigationPage) {
				((NavigationPage)mainPage).PushAsync (page);
			} else {
				throw new Exception ("Cannot navigate! MainPage is not NavigationPage, (SwarmViewModel)");
			}
		}

		public IEnumerable<Agent> GetAgents(){
			return Model.Agents;
		}
	}
}

