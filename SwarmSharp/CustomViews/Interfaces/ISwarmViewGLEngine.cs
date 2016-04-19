using System;
using Xamarin.Forms;

namespace SwarmSharp
{
	public interface ISwarmViewGLEngine
	{
		void Render (Xamarin.Forms.Rectangle r);
		void SetView(OpenGLView view);
		void SetViewModel (PlayfieldViewModel viewModel);
		void ResetSize();
	}
}

