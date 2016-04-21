using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace SwarmSharp
{
	public class MovementRuleBuilderViewModel : ViewModel
	{
		MovementRuleBuilder ruleBuilder;

		public string RuleType {
			get { 
				if (HasMovementRule)
					return ruleBuilder.BuildingName;
				else
					return String.Empty;
			}
			set { changeType (value); }
		}
			
		public bool HasMovementRule { get { return ruleBuilder.BuildingRuleType != null; } }

		public List<string> RuleTypes { get { return MovementRuleBuilder.RuleTypes.Keys.ToList (); } }

		public int RuleIndex { get { return RuleTypes.IndexOf (RuleType); } set { changeType (RuleTypes[value]); } }

		ObservableCollection<TargetViewModel> targets;
		public ObservableCollection<TargetViewModel> Targets { get { return targets; } set { SetProperty (ref targets, value); } }

		ICommand _addRule;
		public ICommand AddRule { get { return _addRule; } set { SetProperty (ref _addRule, value); } }

		ICommand _editRule;
		public ICommand EditRule { get { return _editRule; } set { SetProperty (ref _editRule, value); } }

		public MovementRuleBuilderViewModel () {
			Initialize ();
		}

		public MovementRuleBuilderViewModel (MovementRuleBuilder ruleBuilder) : this () {
			this.ruleBuilder = ruleBuilder;
		}

		void Initialize () {
			ruleBuilder = new MovementRuleBuilder ();
			Targets = new ObservableCollection<TargetViewModel> ();
			AddRule = new Command (addRule);
			EditRule = new Command (editRule);
		}

		void changeType (string name) {
			if (RuleType != name) {
				ruleBuilder.ChangeType (name);
				updateTargets ();
				OnPropertyChanged (nameof (RuleType));
				OnPropertyChanged (nameof (HasMovementRule));
				OnPropertyChanged (nameof (RuleIndex));
			}

		}

		void updateTargets () {
			Targets.Clear ();
			foreach (var target in ruleBuilder.GetTargets()) {
				Targets.Add(new TargetViewModel(target, ruleBuilder));
			}
			OnPropertyChanged (nameof (Targets));
		}

		void addRule () {
			if (!HasMovementRule)
				changeType (RuleTypes.First ());
			EditRule.Execute (null);
		}

		void editRule () {
			if (Application.Current.MainPage is NavigationPage) {
				var page = new RuleConfigurationPage ();
				page.BindingContext = this;
				((NavigationPage)Application.Current.MainPage).PushAsync (page);
			} else {
				throw new Exception ("Main page is not navigation page! (from MovementRuleBuilderViewModel");
			}
		}
	}
}

