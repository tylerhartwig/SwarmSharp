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

		static void checkError(){
			ErrorCode error = GL.GetErrorCode ();
			if (error != ErrorCode.NoError) {
				System.Diagnostics.Debug.WriteLine (error.ToString ());
				throw new Exception (error.ToString ());
			}
		}

		// Buffers
		int depthRenderBuffer;
		int colorRenderBuffer;
		#if __IOS__
		int frameBuffer;
		#endif

		// shape buffers;
		uint circleVertexBuffer;
		uint circleIndexBuffer;
		int circleIndexLength;
		BeginMode circleBeginMode;
		uint diamondVertexBuffer;
		uint diamondIndexBuffer;
		int diamondIndexLength;
		BeginMode diamondBeginMode;
		uint squareVertexBuffer;
		uint squareIndexBuffer;
		int squareIndexLength;
		BeginMode squareBeginMode;
		uint triangleVertexBuffer;
		uint triangleIndexBuffer;
		int triangleIndexLength;
		BeginMode triangleBeginMode;

		// Drawing
		int drawingLength;
		BeginMode drawingMode;

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
			InitView ();
			checkError ();
			InitRenderBuffers ();
			checkError ();
			#if __IOS__
			InitFrameBuffers ();
			checkError ();
			#endif
			InitSimpleShaders ();
			checkError ();
			InitShapeBuffers ();
			checkError ();
			initialized = true;
		}

		void InitView () {
			GL.ClearColor (1f, 1f, 1f, 1f);
			GL.Enable (EnableCap.DepthTest);
			GL.Viewport (0, 0, pixelWidth, pixelHeight);
		}

		void InitRenderBuffers () {
			GL.GenRenderbuffers (1, out depthRenderBuffer);
			GL.BindRenderbuffer (RenderbufferTarget.Renderbuffer, depthRenderBuffer);
			GL.RenderbufferStorage (RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.DepthComponent16, pixelWidth, pixelHeight);

			GL.GenRenderbuffers (1, out colorRenderBuffer);
			GL.BindRenderbuffer (RenderbufferTarget.Renderbuffer, colorRenderBuffer);
			GL.RenderbufferStorage (RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.Rgba4, pixelWidth, pixelHeight);
		}

		void InitShapeBuffers () {
			InitCircleBuffer ();
			checkError ();
			InitDiamondBuffer ();
			checkError ();
			InitSquareBuffer ();
			checkError ();
			InitTriangleBuffer ();
			checkError ();
		}

		public void ResetSize(){
			GL.BindRenderbuffer (RenderbufferTarget.Renderbuffer, depthRenderBuffer);
			GL.RenderbufferStorage (RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.DepthComponent16, pixelWidth, pixelHeight);

			GL.BindRenderbuffer (RenderbufferTarget.Renderbuffer, colorRenderBuffer);
			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.Rgba4, pixelWidth, pixelHeight);
			GL.Viewport (0, 0, pixelWidth, pixelHeight);
		}

		#if __IOS__
		void InitFrameBuffers () {
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

		void InitSimpleShaders () {
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

		void InitCircleBuffer () {
			GL.GenBuffers (1, out circleVertexBuffer);
			checkError ();
			GL.BindBuffer (BufferTarget.ArrayBuffer, circleVertexBuffer);
			checkError ();
			var verticies = new List<Vector3> ();
			var indicies = new List<ushort> ();
			verticies.Add (new Vector3 (0, 0, 0));
			indicies.Add (0);
			for (int i = 0; i <= CIRCLE_RESOLUTION; i++) {
				double angle = i * Math.PI * 2.0 / CIRCLE_RESOLUTION;
				verticies.Add (new Vector3 ((float)(Math.Cos (angle)), (float)(Math.Sin (angle)), 0));
				indicies.Add ((ushort)(i + 1));
			}
			circleIndexLength = CIRCLE_RESOLUTION + 2;
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * verticies.Count), verticies.ToArray (), BufferUsage.StaticDraw);
			checkError ();

			GL.GenBuffers (1, out circleIndexBuffer);
			checkError ();
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, circleIndexBuffer);
			checkError ();
			GL.BufferData (BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(ushort) * indicies.Count), indicies.ToArray (), BufferUsage.StaticDraw);
			checkError ();
			circleBeginMode = BeginMode.TriangleFan;
			
			GL.BindBuffer (BufferTarget.ArrayBuffer, 0);
			checkError ();
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, 0);
			checkError ();
		}

		void InitDiamondBuffer () {
			var verticies = new List<Vector3> ();
//			var indicies = new List<ushort> ();
			verticies.Add(new Vector3(0.5f, 0f, 0f));
			verticies.Add(new Vector3(0f, 0.5f, 0f));
			verticies.Add(new Vector3(-0.5f, 0f, 0f));
			verticies.Add(new Vector3(0f, -0.5f, 0f));
			verticies.Add(new Vector3(0.5f, 0f, 0f));
//			indicies.AddRange (new ushort[] { 0, 1, 2, 0, 2, 3 });
			diamondIndexLength = verticies.Count;
			GL.GenBuffers (1, out diamondVertexBuffer);
			GL.BindBuffer (BufferTarget.ArrayBuffer, diamondVertexBuffer);
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * verticies.Count), verticies.ToArray (), BufferUsage.StaticDraw);

//			GL.GenBuffers (1, out diamondIndexBuffer);
//			GL.BindBuffer (BufferTarget.ElementArrayBuffer, diamondIndexBuffer);
//			GL.BufferData (BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(ushort) * indicies.Count), indicies.ToArray (), BufferUsage.StaticDraw);
			diamondBeginMode = BeginMode.TriangleStrip;
			GL.BindBuffer (BufferTarget.ArrayBuffer, 0);
//			GL.BindBuffer (BufferTarget.ElementArrayBuffer, 0);
		}

		void InitSquareBuffer () {
			var verticies = new List<Vector3> ();
//			var indicies = new List<ushort> ();
			verticies.Add (new Vector3 (0.5f, 0.5f, 0f));
			verticies.Add (new Vector3 (-0.5f, 0.5f, 0f));
			verticies.Add (new Vector3 (-0.5f, -0.5f, 0f));
			verticies.Add (new Vector3 (0.5f, -0.5f, 0f));
			verticies.Add (new Vector3 (0.5f, 0.5f, 0f));
//			indicies.AddRange (new ushort[] { 0, 1, 2, 2, 3, 0 });
			squareIndexLength = verticies.Count;
			GL.GenBuffers (1, out squareVertexBuffer);
			GL.BindBuffer (BufferTarget.ArrayBuffer, squareVertexBuffer);
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * verticies.Count), verticies.ToArray (), BufferUsage.StaticDraw);

//			GL.GenBuffers (1, out squareIndexBuffer);
//			GL.BindBuffer (BufferTarget.ElementArrayBuffer, squareIndexBuffer);
//			GL.BufferData (BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(ushort) * indicies.Count), indicies.ToArray (), BufferUsage.StaticDraw);
			squareBeginMode = BeginMode.TriangleStrip;
			GL.BindBuffer (BufferTarget.ArrayBuffer, 0);
//			GL.BindBuffer (BufferTarget.ElementArrayBuffer, 0);
		}

		void InitTriangleBuffer () {
			var verticies = new List<Vector3> ();
//			var indicies = new List<ushort> ();
			verticies.Add (new Vector3(0f, 0.5f, 0f));
			verticies.Add (new Vector3(-1.0f * (float)(1.0f / Math.Sqrt (3.0f)), -0.5f, 0f));
			verticies.Add (new Vector3((float)(1.0f / Math.Sqrt (3.0f)), -0.5f, 0f));
//			indicies.AddRange (new ushort[] { 0, 1, 2 });
			triangleIndexLength = verticies.Count;

			GL.GenBuffers (1, out triangleVertexBuffer);
			GL.BindBuffer (BufferTarget.ArrayBuffer, triangleVertexBuffer);
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * verticies.Count), verticies.ToArray (), BufferUsage.StaticDraw);

//			GL.GenBuffers (1, out triangleIndexBuffer);
//			GL.BindBuffer (BufferTarget.ElementArrayBuffer, triangleIndexBuffer);
//			GL.BufferData (BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(ushort) * indicies.Count), indicies.ToArray (), BufferUsage.StaticDraw);
			triangleBeginMode = BeginMode.Triangles;
			GL.BindBuffer (BufferTarget.ArrayBuffer, 0);
//			GL.BindBuffer (BufferTarget.ElementArrayBuffer, 0);
		}

		#endregion

		#region Drawing

		void SetColor (AgentColor color) {
			switch (color) {
			case AgentColor.Blue:
				SetColor (0x66, 0x9C, 0xB2);
				break;
			case AgentColor.Green:
				SetColor (0x68, 0xA5, 0x82);
				break;
			case AgentColor.Purple:
				SetColor (0x73, 0x69, 0xAF);
				break;
			case AgentColor.Red:
				SetColor (0xA8, 0x69, 0x69);
				break;
			case AgentColor.Yellow:
				SetColor (0xA3, 0x95, 0x69);
				break;
			}
		}

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

		void SetupShapeBuffer (uint arrayBuffer, uint elementBuffer) {
			GL.BindBuffer (BufferTarget.ArrayBuffer, arrayBuffer);
			checkError ();
//			GL.BindBuffer (BufferTarget.ElementArrayBuffer, elementBuffer);
//			checkError();
			GL.VertexAttribPointer (Shaders.SimpleShader.PositionAttribute, 3,
				VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
		}

		void SetupShape (AgentShape shape) {
			switch (shape) {
			case AgentShape.Circle:
				SetupShapeBuffer (circleVertexBuffer, circleIndexBuffer);
				drawingLength = circleIndexLength;
				drawingMode = circleBeginMode;
				break;
			case AgentShape.Diamond:
				SetupShapeBuffer (diamondVertexBuffer, diamondIndexBuffer);
				drawingLength = diamondIndexLength;
				drawingMode = diamondBeginMode;
				break;
			case AgentShape.Square:
				SetupShapeBuffer (squareVertexBuffer, squareIndexBuffer);
				drawingLength = squareIndexLength;
				drawingMode = squareBeginMode;
				break;
			case AgentShape.Triangle:
				SetupShapeBuffer (triangleVertexBuffer, triangleIndexBuffer);
				drawingLength = triangleIndexLength;
				drawingMode = triangleBeginMode;
				break;
			}
		}

		void DrawShape (int x, int y, int radius = 1) {

			Matrix4 modelView = Matrix4.Scale (radius) * Matrix4.CreateTranslation (x, y, 0);
			GL.UniformMatrix4 (Shaders.SimpleShader.ModelviewUniform, false, ref modelView);
			checkError();

			GL.DrawArrays (drawingMode, 0, drawingLength);
//			GL.DrawElements(drawingMode, drawingLength, DrawElementsType.UnsignedShort, 0);
			checkError();
		}
			
		#endregion

		public void SetView(OpenGLView view) {
			this.view = view;
		}

		public void SetViewModel (PlayfieldViewModel viewModel) {
			this.viewModel = viewModel;
		}

		public void Render (Rectangle r) {
			if(!initialized)
				Init ();
			GL.Clear ((ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

			Matrix4 orthogonal = Matrix4.CreateTranslation(-(float)pixelWidth / 2.0f, -(float)pixelHeight / 2.0f, 0f) * Matrix4.CreateOrthographic (pixelWidth, pixelHeight, -1, 1);
			GL.UniformMatrix4 (Shaders.SimpleShader.ProjectionUniform, false, ref orthogonal);


			foreach (var swarm in viewModel.SwarmViewModels){
				SetupShape (swarm.Shape);
				SetColor (swarm.Color);
				foreach (var agent in swarm.GetAgents()){
					DrawShape ((int)(agent.Position.X * scale), (int)(agent.Position.Y * scale), 3 * (int)scale);
				}
			}
		}
	}
}


