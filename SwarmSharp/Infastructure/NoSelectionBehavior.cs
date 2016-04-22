using System;
using Xamarin.Forms;

namespace SwarmSharp
{
	public class NoSelectionBehavior : BehaviorBase<ListView>
	{
		public NoSelectionBehavior ()
		{
		}

		protected override void OnAttachedTo (ListView bindable)
		{
			base.OnAttachedTo (bindable);
			bindable.ItemSelected += nullSelection;
		}

		protected override void OnDetachingFrom (ListView bindable)
		{
			base.OnDetachingFrom (bindable);
			bindable.ItemSelected -= nullSelection;
		}


		void nullSelection (object sender, SelectedItemChangedEventArgs e){
			((ListView)sender).SelectedItem = null;
		}
	}
}

