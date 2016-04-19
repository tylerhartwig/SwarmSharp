using System;
using Xamarin.Forms;

namespace SwarmSharp
{
	public class DisappearTriggerAction : TriggerAction<VisualElement>
	{
		protected override void Invoke (VisualElement sender)
		{
			sender.IsVisible = false;
		}
	}
}

