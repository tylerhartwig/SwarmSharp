using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SwarmSharp
{
	public class ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public ViewModel () { }

		protected virtual void OnPropertyChanged([CallerMemberName] string name = null) {
			var handler = PropertyChanged;
			if (handler != null)
				handler (this, new PropertyChangedEventArgs (name));
		}

		protected bool SetProperty<T>(ref T data, T value, [CallerMemberName] string name = "") {
			if (Object.Equals (data, value))
				return false;
			data = value;
			OnPropertyChanged (name);
			return true;
		}
	}
}

