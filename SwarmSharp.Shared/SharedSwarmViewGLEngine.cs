using System;
using SwarmSharp.Shared;
using OpenTK.Graphics.ES30;
using Xamarin.Forms;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using OpenTK;
#if __ANDROID__
using Android.Content;
#endif

[assembly: Dependency (typeof (SharedSwarmViewGLEngine))]
namespace SwarmSharp.Shared
{
	public class SharedSwarmViewGLEngine : ISwarmViewGLEngine
	{
		const int CIRCLE_RESOLUTION = 20;


		double scale;

		private static class Shaders {
			public static class SimpleShader {
				public static string VertexShader = "SimpleVertex";
				public static string FragmentShader = "SimpleFragment";
				static string positionAttributeIdentifier = "Position";
				static string sourceColorUniformIdentifier = "SourceColor";
				static string projectionUniformIdentifier = "Projection";
				static string modelviewUniformIdentifier = "Modelview";
				public static int PositionAttribute { get; private set; }
				public static int ColorUniform { get; private set; }
				public static int ProjectionUniform { get; private set; }
				public static int ModelviewUniform { get; private set; }

				public static void Init (int programHandle) {
					PositionAttribute = GL.GetAttribLocation (programHandle, positionAttributeIdentifier);
					ColorUniform = GL.GetUniformLocation (programHandle, sourceColorUniformIdentifier);
					ProjectionUniform = GL.GetUniformLocation (programHandle, projectionUniformIdentifier);
					ModelviewUniform = GL.GetUniformLocation (programHandle, modelviewUniformIdentifier);
				}
			}
		}

		// Buffers
		int depthRenderBuffer;
		int colorRenderBuffer;
		#if __IOS__
		int frameBuffer;
		#endif
		uint circleVertexBuffer;
		uint circleIndexBuffer;

		// Programs
		int simpleProgram;

		private int pixelWidth { get { return (int)(view.Width * scale); } }
		private int pixelHeight { get { return (int)(view.Height * scale); } }
		private bool initialized = false;
		private OpenGLView view;
		private PlayfieldViewModel viewModel;

		#region Initialization

		public SharedSwarmViewGLEngine () { 
			#if __ANDROID__
			scale = Forms.Context.Resources.DisplayMetrics.Density;
			#elif __IOS__
			scale = UIKit.UIScreen.MainScreen.Scale;
			#endif
		}

		void Init (){
			// GL.Viewport (0, 0, pixelWidth, pixelHeight);
			InitRenderBuffers ();
			#if __IOS__
			InitFrameBuffers ();
			#endif
			InitSimpleShaders ();
			InitCircleBuffers ();
			initialized = true;
		}

		void InitRenderBuffers (){
			GL.GenRenderbuffers (1, out depthRenderBuffer);
			GL.BindRenderbuffer (RenderbufferTarget.Renderbuffer, depthRenderBuffer);
			GL.RenderbufferStorage (RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.DepthComponent16, pixelWidth, pixelHeight);

			GL.GenRenderbuffers (1, out colorRenderBuffer);
			GL.BindRenderbuffer (RenderbufferTarget.Renderbuffer, colorRenderBuffer);
			GL.RenderbufferStorage (RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.Rgba4, pixelWidth, pixelHeight);
		}

		#if __IOS__
		void InitFrameBuffers (){
			GL.GenFramebuffers (1, out frameBuffer);
			GL.BindFramebuffer (FramebufferTarget.Framebuffer, frameBuffer);
			GL.FramebufferRenderbuffer (FramebufferTarget.Framebuffer, FramebufferSlot.ColorAttachment0,
				RenderbufferTarget.Renderbuffer, colorRenderBuffer);
			GL.FramebufferRenderbuffer (FramebufferTarget.Framebuffer, FramebufferSlot.DepthAttachment,
				RenderbufferTarget.Renderbuffer, depthRenderBuffer);
		}
		#endif

		int CompileShader(string shaderName, ShaderType shaderType) {
			string prefix;
			#if __IOS__
			prefix = "SwarmSharp.iOS";
			#elif __ANDROID__
			prefix = "SwarmSharp.Droid";
			#endif
			var assembly = typeof(SharedSwarmViewGLEngine).GetTypeInfo ().Assembly;
			var stream = assembly.GetManifestResourceStream (String.Format("{0}.Shaders.{1}.glsl", prefix, shaderName));
			string shaderString;
			using (var reader = new StreamReader (stream)) {
				shaderString = reader.ReadToEnd ();
			}
			int shaderHandle = GL.CreateShader (shaderType);
			GL.ShaderSource (shaderHandle, shaderString);
			GL.CompileShader (shaderHandle);

			return shaderHandle;
		}

		void InitSimpleShaders (){
			int vertexShader = CompileShader (Shaders.SimpleShader.VertexShader, ShaderType.VertexShader);
			int fragmentShader = CompileShader (Shaders.SimpleShader.FragmentShader, ShaderType.FragmentShader);
			simpleProgram = GL.CreateProgram ();
			GL.AttachShader (simpleProgram, vertexShader);
			GL.AttachShader (simpleProgram, fragmentShader);
			GL.LinkProgram (simpleProgram);
			GL.UseProgram (simpleProgram);

			Shaders.SimpleShader.Init (simpleProgram);

			GL.EnableVertexAttribArray (Shaders.SimpleShader.PositionAttribute);
		}

		void InitCircleBuffers () {
			GL.GenBuffers (1, out circleVertexBuffer);
			GL.BindBuffer (BufferTarget.ArrayBuffer, circleVertexBuffer);
			var verticies = new List<Vector3> ();
			var indicies = new List<byte> ();
			verticies.Add (new Vector3 (0, 0, 0));
			indicies.Add (0);
			for (int i = 0; i <= CIRCLE_RESOLUTION; i++){
				double angle = i * Math.PI * 2.0 / CIRCLE_RESOLUTION;
				verticies.Add(new Vector3((float)(Math.Cos(angle)), (float)(Math.Sin(angle)), 0));
				indicies.Add ((byte)(i + 1));
			}
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * verticies.Count), verticies.ToArray (), BufferUsage.StaticDraw);

			GL.GenBuffers (1, out circleIndexBuffer);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, circleIndexBuffer);
			GL.BufferData (BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(byte) * indicies.Count), indicies.ToArray (), BufferUsage.StaticDraw);
		}

		#endregion

		#region Drawing

		void SetColor (int red, int green, int blue, int alpha = 255) {
			SetColor (red / 255f, green / 255f, blue / 255f, alpha / 255f);
		}

		void SetColor (float red, float green, float blue, float alpha = 1f) {
			var color = new Vector4 (red, green, blue, alpha);
			SetColor (color);
		}

		void SetColor (Vector4 color) {
			GL.Uniform4 (Shaders.SimpleShader.ColorUniform, color);
		}

		void DrawCircle(int x, int y, int radius = 1) {
			GL.BindBuffer (BufferTarget.ArrayBuffer, circleVertexBuffer);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, circleIndexBuffer);

			Matrix4 modelView = Matrix4.Scale (radius) * Matrix4.CreateTranslation (x, y, 0);
			GL.UniformMatrix4 (Shaders.SimpleShader.ModelviewUniform, false, ref modelView);

			GL.VertexAttribPointer (Shaders.SimpleShader.PositionAttribute, 3,
				VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);

			GL.DrawElements (BeginMode.TriangleFan, CIRCLE_RESOLUTION + 2, DrawElementsType.UnsignedByte, 0);

		}

		#endregion

		public void SetView(OpenGLView view){
			this.view = view;
		}

		public void SetViewModel (PlayfieldViewModel viewModel) {
			this.viewModel = viewModel;
		}

		public void Render (Rectangle r) {
			if(!initialized)
				Init ();
			GL.ClearColor (1f, 1f, 1f, 1f);
			GL.Clear ((ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
			GL.Enable (EnableCap.DepthTest);
			GL.Viewport (0, 0, pixelWidth, pixelHeight);

			Matrix4 orthogonal = Matrix4.CreateTranslation(-(float)pixelWidth / 2.0f, -(float)pixelHeight / 2.0f, 0f) * Matrix4.CreateOrthographic (pixelWidth, pixelHeight, -1, 1);
			GL.UniformMatrix4 (Shaders.SimpleShader.ProjectionUniform, false, ref orthogonal);

			SetColor (0x2c, 0x3e, 0x50);

			foreach (var group in viewModel.GetAgents()) {
				foreach (var agent in group) {
					DrawCircle ((int)(agent.Position.X * scale), (int)(agent.Position.Y * scale), 3);
				}
			}
		}
	}
}

