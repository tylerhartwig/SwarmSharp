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
				return Model.MovementRule != null;;
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
			get { return swarm.Agents.Count; } 
			set { 
				swarm.SetCount (value);
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

		int selectedRule;
		public int SelectedRule { get { return selectedRule; } set { SetProperty (ref selectedRule, value); } }

		ICommand _editRule;
		public ICommand EditRule { get { return _editRule; } set { SetProperty (ref _editRule, value); } }

		ICommand _addRule;
		public ICommand AddRule { get { return _addRule; } set { SetProperty (ref _addRule, value); } } 

		MovementRuleViewModel movementRule;
		public MovementRuleViewModel MovementRule { get { return movementRule; } set { SetProperty (ref movementRule, value); } }
			
		ObservableCollection<string> ruleOptions;
		public ObservableCollection<string> RuleOptions { get { return ruleOptions; } set { SetProperty (ref ruleOptions, value); } }

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
			

			MovementRule = AgentRuleFactory.Instance.CreateDefaultViewModel ();
			MovementRule.Model = Model.MovementRule;
			RuleOptions = new ObservableCollection<string> ();
			AgentShapes = new ObservableCollection<string> ();
			AgentColors = new ObservableCollection<string> ();

			EditRule = new Command (editRule);
			AddRule = new Command (addRule);

			foreach (var rule in AgentRuleFactory.Instance.GetRuleNames()) {
				RuleOptions.Add (rule);
			}
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
			DataService.CurrentMovementRule = Model.MovementRule;

			if (Application.Current.MainPage is NavigationPage) {
				var page = new RuleConfigurationPage ();
				((NavigationPage)Application.Current.MainPage).PushAsync (page);
			} else {
				throw new Exception ("Main page is not navigation page! cannot navigate (SwarmViewModel)");
			}

		}

		void addRule(){
			if (Model.MovementRule == null)
				Model.MovementRule = AgentRuleFactory.Instance.CreateDefault ();
			OnPropertyChanged (nameof (HasMovementRule));
		}

		protected override void OnPropertyChanged ([CallerMemberName]string name = null)
		{
			base.OnPropertyChanged (name);

			if (name == nameof (SelectedRule)) {
				Model.MovementRule = AgentRuleFactory.Instance.CreateRule (RuleOptions [SelectedRule]);
			}

		}
	}
}

