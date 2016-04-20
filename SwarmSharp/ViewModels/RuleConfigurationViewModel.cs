using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

namespace SwarmSharp
{
	public class RuleConfigurationViewModel : ViewModel
	{
		MovementRuleBuilder ruleBuilder;

		public List<string> RuleTypes { get { return MovementRuleBuilder.RuleTypes.Keys.ToList (); } } 

		public string RuleType { get { return ruleBuilder.BuildingName; }
			set { 
				ruleBuilder.ChangeType (value);
				updateTargets ();
				OnPropertyChanged ();
				OnPropertyChanged (nameof (RuleIndex));
			} 
		}
			
		public int RuleIndex { get { 
				var value = RuleTypes.IndexOf (RuleType); 
				return value;
			} 

			set { 
				ruleBuilder.ChangeType (RuleTypes [value]);
				updateTargets ();
				OnPropertyChanged ();
				OnPropertyChanged (nameof (RuleType));
			} 
		}

		ObservableCollection<TargetViewModel> targets;
		public ObservableCollection<TargetViewModel> Targets { get { return targets; } set { SetProperty (ref targets, value); } }

		public RuleConfigurationViewModel() { 
			Targets = new ObservableCollection<TargetViewModel> ();
			ruleBuilder = DataService.CurrentMovementRuleBuilder;
			updateTargets ();
		}

		void updateTargets (){
			Targets.Clear ();
			foreach (var target in ruleBuilder.GetTargets()) {
				Targets.Add(new TargetViewModel(target, ruleBuilder));
			}
		}
	}
}

