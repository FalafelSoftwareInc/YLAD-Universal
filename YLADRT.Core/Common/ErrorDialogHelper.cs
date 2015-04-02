using System;
using System.Linq;
using Windows.UI.Popups;

namespace YLADRT.Core.Common
{
	public class ErrorDialogHelper
	{
		public static async void Show(string content, string title = "Error")
		{
			MessageDialog messageDialog = new MessageDialog(content, title);
			await messageDialog.ShowAsync();
		}
	}
}