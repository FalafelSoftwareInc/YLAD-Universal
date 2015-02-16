using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using YLADRT.Shared.ViewModel;
using YLADRT.WindowsPhone.Views;

namespace YLADRT.WindowsPhone
{
    /// <summary>
    /// Based on and inspired by ideas by Jeff Wilcox:
    /// http://www.jeff.wilcox.name/2011/07/my-app-about-page/
    /// </summary>
    public partial class AboutPage
    {
		protected AboutViewModel ViewModel
		{
			get { return this.DataContext as AboutViewModel; }
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutPage"/> class.
        /// </summary>
        public AboutPage()
        {
            InitializeComponent();
            Style = (Style)Resources["AboutPageStyle"];
        }

		protected override async void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			await InitializeMainPivotItem();
		}

		public async Task InitializeMainPivotItem()
		{
			// ensure view model exists
			if (ViewModel == null) return;

			// load all the app info
			await ViewModel.LoadAboutData();

			// generate default page instantly
			var defaultItem = new ApplicationInfoView();
			AddPivotItem(ViewModel.MainItemData.Title, defaultItem);

			// create the rest of the pages after the 
			// dialog has loaded (to make it visible faster)
			await InitializeSecondaryPivotItems();
		}

		private void AddPivotItem(string header, object content)
		{
			// create the item
			var item = new PivotItem();

			// set properties and add
			item.Style = (Style)Resources["AboutPivotItemStyle"];
			item.Header = header;
			item.Content = content;
			PivotControl.Items.Add(item);
		}

		private async Task InitializeSecondaryPivotItems()
		{
			foreach (var page in ViewModel.Items)
			{
				// create view model
				var pageViewModel = new GenericItemViewModel(page);

				// load external resource?
				if (page.Uri != null) await pageViewModel.DownloadExternalContent();

				// create view
				var pageView = new GenericItemView();
				pageView.DataContext = pageViewModel;

				// add to pivot control
				AddPivotItem(page.Title, pageView);
			}
		}
    }
}
