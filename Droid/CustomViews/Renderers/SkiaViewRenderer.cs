using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof(SwarmSharp.SkiaView), typeof(SwarmSharp.Droid.SkiaViewRenderer))]
namespace SwarmSharp.Droid
{
	public class SkiaViewRenderer : ViewRenderer<SkiaView, NativeSkiaView>
	{
		NativeSkiaView view;

		public SkiaViewRenderer () { }

		protected override void OnElementChanged (ElementChangedEventArgs<SkiaView> e){
			base.OnElementChanged (e);

			if (Control == null) {
				view = new NativeSkiaView (Context, Element);
				SetNativeControl (view);
			}
		}
	}
}

