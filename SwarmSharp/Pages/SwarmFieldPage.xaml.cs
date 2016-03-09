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

		public async void ButtonClicked(object sender, EventArgs args){
			for (int i = 0; i < 1000; i++) {
				await vm.StartPlayAsync ();
			}
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
		}

	}
}

