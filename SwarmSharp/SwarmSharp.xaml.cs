using System;

using Xamarin.Forms;
using SkiaSharp;

namespace SwarmSharp
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent ();


			MainPage = new ContentPage {
				Content = new SkiaView(Xamagon)
			};
		}

		void Xamagon(SKCanvas canvas, int width, int height){
//			using (var surface = SKSurface.Create(width, height, SKColorType.N_32, SKAlphaType.Premul)){
//				canvas = surface.Canvas;

				// clear the canvas / fill with white
			canvas.Clear (SKColors.Aqua);

				// set up drawing tools
				using (var paint = new SKPaint ()) {
					paint.IsAntialias = true;
					paint.Color = new SKColor (0x2c, 0x3e, 0x50);
					paint.StrokeCap = SKStrokeCap.Round;

					// create the Xamagon path
					using (var path = new SKPath ()) {
						path.MoveTo (71.4311121f, 56f);
						path.CubicTo (68.6763107f, 56.0058575f, 65.9796704f, 57.5737917f, 64.5928855f, 59.965729f);
						path.LineTo (43.0238921f, 97.5342563f);
						path.CubicTo (41.6587026f, 99.9325978f, 41.6587026f, 103.067402f, 43.0238921f, 105.465744f);
						path.LineTo (64.5928855f, 143.034271f);
						path.CubicTo (65.9798162f, 145.426228f, 68.6763107f, 146.994582f, 71.4311121f, 147f);
						path.LineTo (114.568946f, 147f);
						path.CubicTo (117.323748f, 146.994143f, 120.020241f, 145.426228f, 121.407172f, 143.034271f);
						path.LineTo (142.976161f, 105.465744f);
						path.CubicTo (144.34135f, 103.067402f, 144.341209f, 99.9325978f, 142.976161f, 97.5342563f);
						path.LineTo (121.407172f, 59.965729f);
						path.CubicTo (120.020241f, 57.5737917f, 117.323748f, 56.0054182f, 114.568946f, 56f);
						path.LineTo (71.4311121f, 56f);
						path.Close ();

						// draw the Xamagon path
						canvas.DrawPath (path, paint);
					}
				}
//			}
			//return canvas;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

