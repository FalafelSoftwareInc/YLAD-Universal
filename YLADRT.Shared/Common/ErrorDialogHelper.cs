using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace YLADRT.Shared.Common
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
