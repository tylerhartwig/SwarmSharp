using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof(SwarmSharp.SkiaView), typeof(SwarmSharp.iOS.SkiaViewRenderer))]
namespace SwarmSharp.iOS
{
	public class SkiaViewRenderer : ViewRenderer<SkiaView, NativeSkiaView>
	{
		public SkiaViewRenderer () { }

		protected override void OnElementChanged(ElementChangedEventArgs<SkiaView> e){
			base.OnElementChanged (e);

			if (Control == null)
				SetNativeControl (new NativeSkiaView (Element));

		}

	}
}

