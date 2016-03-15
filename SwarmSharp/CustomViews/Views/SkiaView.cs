using System;
using Xamarin.Forms;
using SkiaSharp;
using System.Threading.Tasks;

namespace SwarmSharp
{
	public class SkiaView : View, ISkiaViewController
	{
		public static readonly BindableProperty DrawCallbackProperty = 
			BindableProperty.Create(nameof(DrawCallback), typeof(Action<SKCanvas, int, int>), typeof(SkiaView), null);

		public static readonly BindableProperty DrawingProperty = 
			BindableProperty.Create(nameof(Drawing), typeof(bool), typeof(SkiaView), false);

		public bool Drawing {
			get { return (bool)GetValue (DrawingProperty); }
			set { SetValue (DrawingProperty, value); }
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

		public async Task StartDrawing(){
			while (Drawing) {
				await Task.Run(() => Redraw ());
			}
		}

		public void Redraw(){
			Device.BeginInvokeOnMainThread (() =>
				onRedrawCallback ());
		}

		protected virtual void Draw (SKCanvas canvas) {
			if (DrawCallback != null)
				DrawCallback (canvas, (int) Width, (int) Height);
		}

		protected override async void OnPropertyChanged (string propertyName)
		{
			base.OnPropertyChanged (propertyName);

			switch (propertyName) {
			case nameof(Drawing):
				await StartDrawing ();
				break;
			case nameof(Width):
			case nameof(Height):
				if (BindingContext is PlayfieldViewModel) {
					((PlayfieldViewModel)BindingContext).Width = (int)Width;
					((PlayfieldViewModel)BindingContext).Height = (int)Height;
					((PlayfieldViewModel)BindingContext).Reset ();
				}
				break;
			}
		}
	}
}

