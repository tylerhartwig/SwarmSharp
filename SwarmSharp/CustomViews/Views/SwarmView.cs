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

			this.SizeChanged += (object sender, EventArgs e) => {
				engine.ResetSize();
			};
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

		protected override void InvalidateLayout ()
		{
			base.InvalidateLayout ();
			engine.ResetSize ();
		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			base.LayoutChildren (x, y, width, height);
			engine.ResetSize ();
		}


		protected override void OnSizeAllocated (double width, double height)
		{
			base.OnSizeAllocated (width, height);

			if (viewModel != null) {
				viewModel.Width = (int)width;
				viewModel.Height = (int)height;
				viewModel.Reset ();
				if (!viewModel.IsPlaying) {
					viewModel.Reset ();
				}
			}

			engine.ResetSize ();
		}
	}
}


