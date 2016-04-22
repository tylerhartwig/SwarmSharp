using System;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Graphics;
using Xamarin.Forms;
using SwarmSharp.Droid;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(UbuntuButtonRenderer))]
namespace SwarmSharp.Droid
{
	public class UbuntuButtonRenderer : ButtonRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Button> e)
		{
			base.OnElementChanged (e);

			var button = (Android.Widget.Button)Control;
			Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "Ubuntu-L.ttf");
			button.Typeface = font;
		}
	}
}

