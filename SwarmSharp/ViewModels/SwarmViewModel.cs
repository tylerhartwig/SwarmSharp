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

		public bool HasMovementRule {
			get {
				return Model.MovementRuleBuilder != null;;
			}
		}

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

		public AgentColor Color {
			get { return swarm.Color; } 
			set {
				swarm.Color = value;
				OnPropertyChanged ();
			}
		}

		public AgentShape Shape {
			get { return swarm.Shape; } 
			set {
				swarm.Shape = value;
				OnPropertyChanged ();
			}
		}

		ICommand _editRule;
		public ICommand EditRule { get { return _editRule; } set { SetProperty (ref _editRule, value); } }

		ICommand _addRule;
		public ICommand AddRule { get { return _addRule; } set { SetProperty (ref _addRule, value); } } 

		public string MovementRule { 
			get { 
				if (HasMovementRule)
					return Model.MovementRuleBuilder.BuildingName;
				else
					return null;
			} 
			set {
				Model.MovementRuleBuilder = new MovementRuleBuilder (value);
				OnPropertyChanged ();
			}
		}

		ObservableCollection<string> agentColors;
		public ObservableCollection<string> AgentColors { get { return agentColors; } 
			set { SetProperty (ref agentColors, value); } }

		ObservableCollection<string> agentShapes;
		public ObservableCollection<string> AgentShapes { get { return agentShapes; } 
			set { SetProperty (ref agentShapes, value); } }

		public SwarmViewModel(){
			Model = DataService.CurrentSwarm;
			Initialize ();
		}
				
		public SwarmViewModel (Swarm model = null){
			Model = model;			
			Initialize ();
		}

		void Initialize(){
			AgentShapes = new ObservableCollection<string> ();
			AgentColors = new ObservableCollection<string> ();

			EditRule = new Command (editRule);
			AddRule = new Command (addRule);
				
			foreach (var val in Enum.GetValues (typeof(AgentColor)).Cast<AgentColor> ().ToList ()) {
				AgentColors.Add (val.ToString ());
			}
			foreach (var val in Enum.GetValues (typeof(AgentShape)).Cast<AgentShape> ().ToList ()) {
				AgentShapes.Add (val.ToString ());
			}
		}

		public IEnumerable<Agent> GetAgents(){
			return Model.Agents;
		}

		void editRule(){
			DataService.CurrentMovementRuleBuilder = Model.MovementRuleBuilder;

			if (Application.Current.MainPage is NavigationPage) {
				var page = new RuleConfigurationPage ();
				((NavigationPage)Application.Current.MainPage).PushAsync (page);
			} else {
				throw new Exception ("Main page is not navigation page! cannot navigate (SwarmViewModel)");
			}
		}

		void addRule(){
			if (!HasMovementRule)
				Model.MovementRuleBuilder = new MovementRuleBuilder ();
			editRule ();
			OnPropertyChanged(nameof(HasMovementRule));
			OnPropertyChanged (nameof (MovementRule));
		}
	}
}

