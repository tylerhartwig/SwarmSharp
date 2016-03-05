﻿using System;
using Xamarin.Forms;
using SkiaSharp;

namespace SwarmSharp
{
	public interface ISkiaViewController : IViewController
	{
		void SendDraw(SKCanvas canvas);
	}
}

