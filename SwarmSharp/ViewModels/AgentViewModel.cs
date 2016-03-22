using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace SwarmSharp
{
	public class AgentViewModel : ViewModel
	{
		string name;
		public string Name { get { return name; } set { SetProperty (ref name, value); } } 

		int count;
		public int Count { get { return count; } set { SetProperty (ref count, value); } } 

		AgentColor color;
		public AgentColor Color { get { return color; } set { SetProperty (ref color, value); } } 

		AgentShape shape;
		public AgentShape Shape { get { return shape; } set { SetProperty (ref shape, value); } } 

		ObservableCollection<string> rules = new ObservableCollection<string> ();
		public ObservableCollection<string> Rules { get { return rules; } set { SetProperty (ref rules, value); } }

		List<AgentColor> agentColors;
		public List<AgentColor> AgentColors { get { return agentColors; } 
			set { SetProperty (ref agentColors, value); } }

		List<AgentShape> agentShapes;
		public List<AgentShape> AgentShapes { get { return agentShapes; } 
			set { SetProperty (ref agentShapes, value); } }
	
		public AgentViewModel () {
			Name = "Default Name";
			Count = 1000;
			Color = AgentColor.Blue;
			Shape = AgentShape.Circle;
			foreach (var name in AgentRuleFactory.Instance.GetRuleNames ()) {
				Rules.Add (name);
			}

		}

		public void Test () {
			AgentColors = Enum.GetValues (typeof(AgentColor)).Cast<AgentColor> ().ToList ();
			AgentShapes = Enum.GetValues (typeof(AgentShape)).Cast<AgentShape> ().ToList ();
		}
	}
}

