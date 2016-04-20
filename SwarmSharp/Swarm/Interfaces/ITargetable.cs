using System;
using System.Collections.Generic;

namespace SwarmSharp
{
	public interface ITargetable
	{
		string Name { get; }
		IEnumerable<Point> GetTargets ();
	}
}

