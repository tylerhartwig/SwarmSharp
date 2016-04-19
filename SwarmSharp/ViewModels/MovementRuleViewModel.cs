using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using Xamarin.Forms;
using System.Collections.Generic;

namespace SwarmSharp
{
	public class MovementRuleViewModel : ViewModel
	{
		IAgentMovementRule rule;
		public IAgentMovementRule Model { get { return rule; } set { SetProperty (ref rule, value); } }

		public string Type { 
			get { 
				return ((AgentRule)rule).Name;
			}  
			set {
				changeType (value);
			}
		}

		List<TargetViewModel> targets;
		public List<TargetViewModel> Targets { get { 
				foreach (var target in rule.TargetList) {
					targets.Add (new TargetViewModel (target, rule));
				}
				return targets; 
			} 
			set { SetProperty (ref targets, value); } 
		}

		public int NumTargets{
			get { return rule.NumTargets; }
		}

		public MovementRuleViewModel () { 
			Targets = new List<TargetViewModel> ();

		}
			
		void changeType (string type){
			rule = AgentRuleFactory.Instance.CreateRule (type);

			OnPropertyChanged (nameof (Type));
			OnPropertyChanged (nameof (Targets));
			OnPropertyChanged (nameof (NumTargets));
		}
	}
}

