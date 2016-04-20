using System;
using System.Collections.ObjectModel;

namespace SwarmSharp
{
	public class TargetViewModel : ViewModel
	{
		string name;
		public string Name { get { return name; } set { SetProperty(ref name, value); } }

		MovementRuleBuilder ruleBuilder;

		public int SelectedTarget { 
			get {
				var ruleTarget = ruleBuilder.GetTargetReference (Name);
				if (ruleTarget != null) {
					return Options.IndexOf (ruleTarget);
				} else {
					ruleBuilder.SetTarget (Name, DataService.CurrentPlayField.Swarms [0]);
					return 0;
				}
			}
			set {
				ruleBuilder.SetTarget (Name, DataService.CurrentPlayField.Swarms [value]);
				OnPropertyChanged ();
			}
		}

		ObservableCollection<string> options;
		public ObservableCollection<string> Options { get { return options; } set { SetProperty(ref options, value); } }

		public TargetViewModel (string name, MovementRuleBuilder ruleBuilder)
		{
			this.ruleBuilder = ruleBuilder;
			Name = name;
			Options = new ObservableCollection<string> ();
			foreach (var swarm in DataService.CurrentPlayField.Swarms){
				Options.Add (swarm.Name);
			}
		}
	}
}

