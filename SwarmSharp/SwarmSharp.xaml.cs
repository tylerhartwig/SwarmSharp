﻿using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Diagnostics;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SwarmSharp
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent ();

//			MainPage = new SwarmFieldPage ();
			DataService.CurrentPlayField = new Playfield ();
			MainPage = new NavigationPage (new PlayfieldConfigurationPage ());
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

