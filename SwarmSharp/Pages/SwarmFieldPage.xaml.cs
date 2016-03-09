using System;
using System.Collections.Generic;
using SkiaSharp;

using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SwarmSharp
{
	public partial class SwarmFieldPage : ContentPage
	{

		public PlayfieldViewModel vm { get; set; }
		public SwarmFieldPage ()
		{
			this.BindingContext = this;
			vm = new PlayfieldViewModel ();
			InitializeComponent ();
		}

		public void ButtonClicked(object sender, EventArgs args){
			vm.Play ();
			Debug.WriteLine (DebugView.NeedRedraw);
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
		}

	}
}

