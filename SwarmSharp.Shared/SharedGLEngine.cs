using System;
using SwarmSharp.Shared;
using OpenTK.Graphics.ES30;

[assembly: Xamarin.Forms.Dependency (typeof (SharedGLEngine))]

namespace SwarmSharp.Shared
{
	public class SharedGLEngine : IGLEngine
	{
		float red, green, blue;

		public SharedGLEngine () { }

		public void Render (Xamarin.Forms.Rectangle r) {
			GL.ClearColor (red, green, blue, 1.0f);
			GL.Clear ((ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

			red += 0.01f;
			if (red >= 1.0f)
				red -= 1.0f;
			green += 0.02f;
			if (green >= 1.0f)
				green -= 1.0f;
			blue += 0.03f;
			if (blue >= 1.0f)
				blue -= 1.0f;
		}
	}
}

