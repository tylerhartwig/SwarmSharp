using System;
using System.Collections.ObjectModel;

namespace SwarmSharp
{
	public class TargetViewModel : ViewModel
	{
		string name;
		public string Name { get { return name; } set { SetProperty(ref name, value); } }

		IAgentMovementRule rule;

//		public int SelectedTarget { get { return selectedTarget; } set { SetProperty (ref selectedTarget, value); } }
		public int SelectedTarget { 
			get {
				var ruleTarget = rule.GetTarget (Name);
				if (ruleTarget != null) {
					return Swarms.IndexOf (rule.GetTarget (Name).Name);
				} else {
					rule.SetTarget (Name, DataService.CurrentPlayField.Swarms [0]);
					return 0;
				}
			}
			set {
				rule.SetTarget (Name, DataService.CurrentPlayField.Swarms [value]);
				OnPropertyChanged ();
			}
		}

		ObservableCollection<string> swarms;
		public ObservableCollection<string> Swarms { get { return swarms; } set { SetProperty(ref swarms, value); } }

		public TargetViewModel (string name, IAgentMovementRule rule)
		{
			this.rule = rule;
			Name = name;
			Swarms = new ObservableCollection<string> ();
			foreach (var swarm in DataService.CurrentPlayField.Swarms){
				Swarms.Add (swarm.Name);
			}
		}
	}
}

