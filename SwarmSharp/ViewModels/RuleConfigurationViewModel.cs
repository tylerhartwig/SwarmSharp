using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

namespace SwarmSharp
{
	public class RuleConfigurationViewModel : ViewModel
	{
		MovementRuleViewModel ruleViewModel;
		public MovementRuleViewModel EditViewModel { get { return ruleViewModel; } set { SetProperty (ref ruleViewModel, value); } }

		ObservableCollection<string> ruleTypes;
		public ObservableCollection<string> RuleTypes { get { return ruleTypes; } set { SetProperty(ref ruleTypes, value); } }

		ObservableCollection<SwarmViewModel> swarmViewModels;
		public ObservableCollection<SwarmViewModel> SwarmViewModels { get { return swarmViewModels; } set { SetProperty (ref swarmViewModels, value); } }

		public string RuleType { get { return EditViewModel.Type; } 
			set { 
				EditViewModel.Type = value;
				OnPropertyChanged ();
				OnPropertyChanged (nameof (Targets));
			} 
		}

		public List<TargetViewModel> Targets { get { return EditViewModel.Targets; } }

		public int RuleIndex { get { return RuleTypes.IndexOf(((AgentRule)EditViewModel.Model).Name); } 
			set { 
				EditViewModel.Model = AgentRuleFactory.Instance.CreateRule (RuleTypes[value]);
				OnPropertyChanged ();
				OnPropertyChanged (nameof (Targets));
			} 
		}

		public RuleConfigurationViewModel(){
			RuleTypes = new ObservableCollection<string> (AgentRuleFactory.Instance.GetRuleNames ());
			EditViewModel = AgentRuleFactory.Instance.CreateRuleViewModel (RuleTypes.FirstOrDefault ());
			EditViewModel.Model = DataService.CurrentMovementRule;
			SwarmViewModels = new ObservableCollection<SwarmViewModel> ();
			foreach (var swarm in DataService.CurrentPlayField.Swarms) {
				SwarmViewModels.Add (new SwarmViewModel (swarm));
			}
		}

//		public RuleConfigurationViewModel (MovementRuleViewModel ruleViewModel) {
//			this.EditViewModel = ruleViewModel;
//			RuleTypes = new ObservableCollection<string> (AgentRuleFactory.Instance.GetRuleNames ());
//		}

		protected override void OnPropertyChanged ([CallerMemberName] string name = null)
		{
			base.OnPropertyChanged (name);
		}
	}
}

