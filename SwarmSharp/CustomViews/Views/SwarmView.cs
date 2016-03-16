using System;

using Xamarin.Forms;

namespace SwarmSharp
{
	public class SwarmView : ContentView
	{
		OpenGLView glView = new OpenGLView ();

		public SwarmView ()
		{
			var engine = DependencyService.Get<IGLEngine> ();
			glView.OnDisplay = engine.Render;
			Content = glView;
			glView.HasRenderLoop = true;
			glView.Display ();
		}
	}
}


