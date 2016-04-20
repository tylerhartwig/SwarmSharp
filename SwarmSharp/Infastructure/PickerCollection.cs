using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace SwarmSharp
{	
	public class PickerCollection
	{
		public static BindableProperty ItemsProperty =
			BindableProperty.CreateAttached("Items", typeof(IEnumerable<string>), typeof(PickerCollection),
				null, /* default value */
				BindingMode.OneWay,
				propertyChanged: ItemsChanged);

		public static IEnumerable<string> GetItems(BindableObject bo)
		{
			return (IEnumerable<string>)bo.GetValue(ItemsProperty);
		}

		public static void SetItems(BindableObject bo, IEnumerable<string> value)
		{
			bo.SetValue(ItemsProperty, value);
		}

		public static void ItemsChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			var picker = bindableObject as Picker;

			if (picker == null)
				return;

			var oldValues = oldValue as IEnumerable<string>;
			var newValues = newValue as IEnumerable<string>;

			if (oldValues != null && newValues != null && newValues.SequenceEqual(oldValues))
				return;


			if (newValue == null)
				return;

			picker.Items.Clear ();

			foreach (var item in (IEnumerable<string>) newValue) {
				picker.Items.Add (item);
			}
		
		}
	}
}

