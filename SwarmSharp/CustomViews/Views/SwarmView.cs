using System;

using Xamarin.Forms;

namespace SwarmSharp
{
	public class SwarmView : ContentView
	{
		ISwarmViewGLEngine engine;
		OpenGLView glView = new OpenGLView ();
		PlayfieldViewModel viewModel;

		public SwarmView ()
		{
			engine = DependencyService.Get<ISwarmViewGLEngine> ();
			engine.SetView (glView);
			Content = glView;
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();
			viewModel = (PlayfieldViewModel)BindingContext;
			engine.SetViewModel (viewModel);
			viewModel.Reset ();
			glView.OnDisplay = engine.Render;
			glView.HasRenderLoop = true;
			glView.Display ();
		}

		protected override void OnSizeAllocated (double width, double height)
		{
			base.OnSizeAllocated (width, height);

			if (viewModel != null) {
				viewModel.Width = (int)width;
				viewModel.Height = (int)height;
				viewModel.Reset ();
			}
		}
	}
}


