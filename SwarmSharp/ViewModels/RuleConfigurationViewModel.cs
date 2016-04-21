using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

namespace SwarmSharp
{
	public class RuleConfigurationViewModel : ViewModel
	{
		MovementRuleBuilderViewModel movementRule;
		public MovementRuleBuilderViewModel MovementRule { get { return movementRule; } set { SetProperty (ref movementRule, value); } }
			
	

		public RuleConfigurationViewModel() { 
			MovementRule = new MovementRuleBuilderViewModel ();
		}


	}
}

