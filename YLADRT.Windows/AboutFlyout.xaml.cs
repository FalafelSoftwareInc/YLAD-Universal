using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YLADRT.Shared.ViewModel;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace YLADRT.Windows
{
	public sealed partial class AboutFlyout : SettingsFlyout
	{

		protected AboutViewModel ViewModel
		{
			get { return this.DataContext as AboutViewModel; }
		}

		public AboutFlyout()
		{
			this.InitializeComponent();
			this.DataContext = new AboutViewModel();

			this.Loaded += AboutFlyout_Loaded;
		}

		async void AboutFlyout_Loaded(object sender, RoutedEventArgs e)
		{
			await ViewModel.LoadAboutData();
		}
	}
}