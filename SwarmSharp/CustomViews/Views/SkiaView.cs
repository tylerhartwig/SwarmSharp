using System;
using Xamarin.Forms;
using SkiaSharp;

namespace SwarmSharp
{
	public class SkiaView : View, ISkiaViewController
	{
		public static readonly BindableProperty DrawCallbackProperty = 
			BindableProperty.Create(nameof(DrawCallback), typeof(Action<SKCanvas, int, int>), typeof(SkiaView), null);

		public static readonly BindableProperty NeedRedrawProperty = 
			BindableProperty.Create(nameof(NeedRedraw), typeof(bool), typeof(SkiaView), false);

		public bool NeedRedraw {
			get { return (bool)GetValue (NeedRedrawProperty); }
			set { SetValue (NeedRedrawProperty, value); }
		}

		public Action<SKCanvas, int, int> DrawCallback {
			get { return (Action<SKCanvas, int, int>)GetValue (DrawCallbackProperty); }
			set { SetValue (DrawCallbackProperty, value); }
		}

		Action onRedrawCallback;

		public SkiaView () { } 

		void ISkiaViewController.SendDraw (SKCanvas canvas) {
			Draw (canvas);
		}

		void ISkiaViewController.SetRedraw (Action recallCallback){
			this.onRedrawCallback = recallCallback;
		}

		public void Redraw(){
			Device.BeginInvokeOnMainThread (() =>
				onRedrawCallback ());
		}

		protected virtual void Draw (SKCanvas canvas) {
			if (DrawCallback != null)
				DrawCallback (canvas, (int) Width, (int) Height);
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			if (BindingContext is PlayfieldViewModel) {
				((PlayfieldViewModel)BindingContext).Width = (int)Width;
				((PlayfieldViewModel)BindingContext).Height = (int)Height;
				((PlayfieldViewModel)BindingContext).Reset ();
			}
		}

		protected override void OnPropertyChanged (string propertyName)
		{
			base.OnPropertyChanged (propertyName);

			System.Diagnostics.Debug.WriteLine (propertyName + " Property changed!");
			Redraw ();
		}
	}
}

