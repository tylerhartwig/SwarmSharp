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
			get { return AgentColors [ColorIndex]; }
			set { ColorIndex = AgentColors.IndexOf (value); }
		}

		public AgentShape Shape {
			get { return AgentShapes [ShapeIndex]; }
			set { ShapeIndex = AgentShapes.IndexOf (value); }
		}

		ICommand _editSwarm;
		public ICommand EditSwarm { get { return _editSwarm; } set { SetProperty (ref _editSwarm, value); } }

		MovementRuleBuilderViewModel movementRule;
		public MovementRuleBuilderViewModel MovementRule { get { return movementRule; }
			set { SetProperty (ref movementRule, value); } }

		int colorIndex;
		public int ColorIndex { get { return colorIndex; } 
			set { 
				SetProperty (ref colorIndex, value); 
				OnPropertyChanged (nameof (Color));
			} 
		}

		ObservableCollection<AgentColor> agentColors;
		public ObservableCollection<AgentColor> AgentColors { get { return agentColors; } set { SetProperty (ref agentColors, value); } }

		ObservableCollection<string> colors;
		public ObservableCollection<string> Colors { get { return colors; } 
			set { SetProperty (ref colors, value); } }

		int shapeIndex;
		public int ShapeIndex { get { return shapeIndex; } 
			set { SetProperty (ref shapeIndex, value); 
				OnPropertyChanged (nameof (Shape));
			} 
		}

		ObservableCollection<AgentShape> agentShapes;
		public ObservableCollection<AgentShape> AgentShapes { get { return agentShapes; } set { SetProperty (ref agentShapes, value); } }

		ObservableCollection<string> shapes;
		public ObservableCollection<string> Shapes { get { return shapes; } 
			set { SetProperty (ref shapes, value); } }

		public SwarmViewModel() : this(new Swarm()) { }
				
		public SwarmViewModel (Swarm model){
			Model = model;			
			MovementRule = new MovementRuleBuilderViewModel (Model.MovementRuleBuilder);
			Initialize ();
		}

		void Initialize(){
			EditSwarm = new Command (editSwarm);
			Shapes = new ObservableCollection<string> ();
			Colors = new ObservableCollection<string> ();
			AgentShapes = new ObservableCollection<AgentShape> ();
			AgentColors = new ObservableCollection<AgentColor> ();

			foreach (var val in Enum.GetValues (typeof(AgentColor)).Cast<AgentColor> ().ToList ()) {
				Colors.Add (val.ToString ());
				AgentColors.Add (val);
			}
			foreach (var val in Enum.GetValues (typeof(AgentShape)).Cast<AgentShape> ().ToList ()) {
				Shapes.Add (val.ToString ());
				AgentShapes.Add (val);
			}

			var random = new Random ();
			ColorIndex = random.Next (Colors.Count);
			ShapeIndex = random.Next (Shapes.Count);			
		}

		void editSwarm () {
			var page = new SwarmConfigurationPage ();
			page.BindingContext = this;
			var mainPage = Application.Current.MainPage;

			if (mainPage is NavigationPage) {
				((NavigationPage)mainPage).PushAsync (page);
			} else {
				throw new Exception ("Cannot navigate! MainPage is not NavigationPage, (SwarmViewModel)");
			}
		}

		public IEnumerable<Agent> GetAgents(){
			return Model.Agents;
		}
	}
}

