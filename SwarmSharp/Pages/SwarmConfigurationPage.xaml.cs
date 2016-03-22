using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SwarmSharp
{
	public partial class SwarmConfigurationPage : ContentPage
	{
		public SwarmConfigurationPage ()
		{
			InitializeComponent ();
			var sampleData = new List<AgentViewModel> ();
			sampleData.Add (new AgentViewModel ());
			//sampleData.Add (new AgentViewModel ());
			listView.ItemsSource = sampleData;
		}
	}
}

