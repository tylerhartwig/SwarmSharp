using System;
using System.Collections.Generic;

namespace SwarmSharp
{
	public class Playfield
	{
		public int Width { get; set; }
		public int Height { get; set; }

		private List<Agent[]> Groups = new List<Agent[]>();

		public Playfield () { }
	}
}

