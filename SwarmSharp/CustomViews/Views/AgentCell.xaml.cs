using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;

namespace SwarmSharp
{
	public partial class AgentCell : ViewCell
	{
		public static readonly BindableProperty ShapeListProperty = BindableProperty.Create ("ShapeList", typeof(List<string>), typeof(AgentCell), new List<string> ());

		public List<string> ShapeList {
			get { return (List<string>)GetValue (ShapeListProperty); }
			set { SetValue (ShapeListProperty, value); }
		}

		public static readonly BindableProperty ColorListProperty = BindableProperty.Create("ColorList", typeof(List<string>), typeof(AgentCell), new List<string> ());

		public List<string> ColorList {
			get { return (List<string>)GetValue (ColorListProperty); }
			set { SetValue (ColorListProperty, value); }
		}


		public AgentCell ()
		{
			InitializeComponent ();
			//foreach (var shape in AgentShapes) {
//				ShapePicker.Items.Add (shape.ToString ());
			//}
			//foreach (var color in AgentColors) {
//				ColorPicker.Items.Add (color.ToString ());
			//}
//			ShapePicker.SelectedIndex = 0;
//			ColorPicker.SelectedIndex = 0;
		}

		private void resetColorList () {
			if(ColorList != null)
			foreach (var color in ColorList) {
				ColorPicker.Items.Clear ();
				ColorPicker.Items.Add (color);
			}
		}

		private void resetShapeList () {
			if (ShapeList != null)
			foreach (var shape in ShapeList) {
				ShapePicker.Items.Clear ();
				ShapePicker.Items.Add (shape);
			}
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			if (BindingContext != null) {
				resetColorList ();
				resetShapeList ();
			}
		}

		protected override void OnPropertyChanged (string propertyName)
		{
			base.OnPropertyChanged (propertyName);

			switch (propertyName) {
			case nameof(ShapeList):
				resetShapeList ();
				break;
			case nameof(ColorList):
				resetColorList ();
				break;
			}
		}
	}
}

