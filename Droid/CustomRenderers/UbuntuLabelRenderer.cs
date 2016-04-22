using System;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Graphics;
using Xamarin.Forms;
using SwarmSharp.Droid;

[assembly: ExportRenderer (typeof (Label), typeof(UbuntuLabelRenderer))]
namespace SwarmSharp.Droid
{
	public class UbuntuLabelRenderer : LabelRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Label> e)
		{
			base.OnElementChanged (e);
			var formsElement = e.NewElement;
			System.Diagnostics.Debug.WriteLine (formsElement.FontFamily);
			var label = (TextView)Control;
			Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "Ubuntu-L.ttf");
			label.Typeface = font;
		}
	}
}

