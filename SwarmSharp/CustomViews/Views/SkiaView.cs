using System;
using Xamarin.Forms;
using SkiaSharp;

namespace SwarmSharp
{
	public class SkiaView : View, ISkiaViewController
	{
		Action <SKCanvas, int, int> onDrawCallback;
		Action onRedrawCallback;

		public SkiaView (Action <SKCanvas, int, int> onDrawCallback) {
			this.onDrawCallback = onDrawCallback;
		}

		void ISkiaViewController.SendDraw (SKCanvas canvas) {
			Draw (canvas);
		}

		void ISkiaViewController.SetRedraw (Action recallCallback){
			this.onRedrawCallback = recallCallback;
		}

		void ISkiaViewController.Redraw (){
			Device.BeginInvokeOnMainThread (() =>
				onRedrawCallback ());
		}

		protected virtual void Draw (SKCanvas canvas) {
			onDrawCallback (canvas, (int) Width, (int) Height);
		}
	}
}

