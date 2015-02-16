using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace YLADRT.Shared.ViewModel
{
	/// <summary>
	/// A simple base class for view models that implements the <c>INotifyPropertyChanged</c> interface.
	/// </summary>
	public class BaseViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises the property changed event.
		/// </summary>
		/// <param name="propertyName">The name of the property that has changed.</param>
		[SuppressMessage("Microsoft.Design",
			"CA1030:UseEventsWhereAppropriate",
			Justification = "This is designed intentionally that way, to enable derived classes to raise the event.")]
		protected void RaisePropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}