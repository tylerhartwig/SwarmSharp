using System;

using Xamarin.Forms;
using SkiaSharp;
using System.Collections.Generic;
using System.Diagnostics;

namespace SwarmSharp
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent ();


			MainPage = new SwarmFieldPage ();
		}

		protected override void OnStart ()
		{
			
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

