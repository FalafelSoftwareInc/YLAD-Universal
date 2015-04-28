using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Storage;
using Windows.UI.Xaml;
using YLADRT.Core.Commands;
using YLADRT.Core.Common;
using YLADRT.Core.Models;

namespace YLADRT.Core.ViewModel
{
	/// <summary>
	/// A view model for the about page and its items.
	/// </summary>
	public sealed class AboutViewModel : BaseViewModel
	{
		private readonly ObservableCollection<LinkData> links = new ObservableCollection<LinkData>();
		private readonly ObservableCollection<ItemData> items = new ObservableCollection<ItemData>();

		private MainItemData mainItemData;
		private string applicationTitle;
		private string applicationTitleUpper;
		private string applicationProductId;
		private string applicationFullVersionProductId;
		private string applicationVersion;
		private string applicationAuthor;
		private string applicationPublisher;
		private string applicationDescription;
		private string additionalNotes;
		private bool isReviewAllowed = true;
		private bool isBuyAllowed;
		private bool forceBuyButton;
		private Visibility buyOptionVisibility;

		#region Properties

		/// <summary>
		/// Gets or sets the main item data that is displayed in the first pivot item.
		/// </summary>
		/// <value>
		/// The main page data.
		/// </value>
		public MainItemData MainItemData
		{
			get
			{
				return mainItemData;
			}

			set
			{
				if (mainItemData != value)
				{
					mainItemData = value;
					RaisePropertyChanged("MainItemData");
				}
			}
		}

		/// <summary>
		/// Gets the list of items that should be shown.
		/// </summary>
		public ObservableCollection<ItemData> Items
		{
			get
			{
				return items;
			}
		}

		/// <summary>
		/// Gets the links that should be added to the list of links on the main pivot item.
		/// </summary>
		public ObservableCollection<LinkData> Links
		{
			get
			{
				return links;
			}
		}

		/// <summary>
		/// Gets or sets the description of the application.
		/// </summary>
		public string ApplicationDescription
		{
			get
			{
				return applicationDescription;
			}

			set
			{
				if (applicationDescription != value)
				{
					applicationDescription = value;
					RaisePropertyChanged("ApplicationDescription");
				}
			}
		}

		/// <summary>
		/// Gets or sets the publisher of the application.
		/// </summary>
		public string ApplicationPublisher
		{
			get
			{
				return applicationPublisher;
			}

			set
			{
				if (applicationPublisher != value)
				{
					applicationPublisher = value;
					RaisePropertyChanged("ApplicationPublisher");
				}
			}
		}

		/// <summary>
		/// Gets or sets the author of the application.
		/// </summary>
		public string ApplicationAuthor
		{
			get
			{
				return applicationAuthor;
			}

			set
			{
				if (applicationAuthor != value)
				{
					applicationAuthor = value;
					RaisePropertyChanged("ApplicationAuthor");
				}
			}
		}

		/// <summary>
		/// Gets or sets the version of the application.
		/// </summary>
		public string ApplicationVersion
		{
			get
			{
				return applicationVersion;
			}

			set
			{
				if (applicationVersion != value)
				{
					applicationVersion = value;
					RaisePropertyChanged("ApplicationVersion");
				}
			}
		}

		/// <summary>
		/// Gets or sets the title of the application.
		/// </summary>
		public string ApplicationTitle
		{
			get
			{
				return applicationTitle;
			}

			set
			{
				if (applicationTitle != value)
				{
					applicationTitle = value;
					RaisePropertyChanged("ApplicationTitle");
				}
			}
		}

		/// <summary>
		/// Gets or sets the application title converted to upper case.
		/// </summary>
		public string ApplicationTitleUpper
		{
			get
			{
				return applicationTitleUpper;
			}

			set
			{
				if (applicationTitleUpper != value)
				{
					applicationTitleUpper = value;
					RaisePropertyChanged("ApplicationTitleUpper");
				}
			}
		}

		/// <summary>
		/// Gets or sets the product ID of the application.
		/// </summary>
		public string ApplicationProductId
		{
			get
			{
				return applicationProductId;
			}

			set
			{
				if (applicationProductId != value)
				{
					applicationProductId = value;
					RaisePropertyChanged("ApplicationProductId");
				}
			}
		}

		/// <summary>
		/// Gets or sets the alternate product ID of the application's full version.
		/// </summary>
		public string ApplicationFullVersionProductId
		{
			get
			{
				return applicationFullVersionProductId;
			}

			set
			{
				if (applicationFullVersionProductId != value)
				{
					applicationFullVersionProductId = value;
					RaisePropertyChanged("ApplicationFullVersionProductId");
				}
			}
		}

		/// <summary>
		/// Gets or sets the additional notes that should be displayed on the main item.
		/// </summary>
		public string AdditionalNotes
		{
			get
			{
				return additionalNotes;
			}

			set
			{
				if (additionalNotes != value)
				{
					additionalNotes = value;
					RaisePropertyChanged("AdditionalNotes");
				}
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="AboutViewModel"/> class.
		/// </summary>
		public AboutViewModel()
		{
			LoadManifestData();
			//LoadDesignData();

			ReviewCommand = new RelayCommand(Review, IsReviewAllowed);
			BuyCommand = new RelayCommand(Buy, IsBuyAllowed);
			BuyOptionVisibility = Visibility.Collapsed;
		}

		#region ReviewCommand

		/// <summary>
		/// Gets the command that wraps the review application operation.
		/// </summary>
		public RelayCommand ReviewCommand { get; private set; }

		private async void Review()
		{
			// should be covered by the command's CanExecute functionality
			if (!isReviewAllowed)
			{
				return;
			}

			try
			{
				// disable review feature
				isReviewAllowed = false;
				ReviewCommand.RaiseCanExecuteChanged();

				// simply show the review task
				await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
			}
			catch (Exception ex)
			{
				ErrorDialogHelper.Show("An error occurred while opening the marketplace: " + ex.ToString());

				// in case of errors, allow the review feature again
				isReviewAllowed = true;
				ReviewCommand.RaiseCanExecuteChanged();
			}
		}

		private bool IsReviewAllowed()
		{
			return isReviewAllowed;
		}

		#endregion

		#region BuyCommand

		/// <summary>
		/// Gets the command that wraps the buy application operation.
		/// </summary>
		public RelayCommand BuyCommand { get; private set; }

		/// <summary>
		/// Gets or sets whether the buy options in the UI should be visible or not.
		/// </summary>
		public Visibility BuyOptionVisibility
		{
			get
			{
				return buyOptionVisibility;
			}

			set
			{
				if (buyOptionVisibility != value)
				{
					buyOptionVisibility = value;
					RaisePropertyChanged("BuyOptionVisibility");
				}
			}
		}

		private async void Buy()
		{
			// should be covered by the command's CanExecute functionality
			if (!isBuyAllowed)
			{
				return;
			}

			try
			{
				// disable buy feature
				isBuyAllowed = false;
				BuyCommand.RaiseCanExecuteChanged();

				// simply show the review task
				await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:navigate?appid=" + CurrentApp.AppId));
			}
			catch (Exception ex)
			{
				ErrorDialogHelper.Show("An error occurred while opening the marketplace: " + ex.ToString());

				// in case of errors, allow the buy feature again
				isBuyAllowed = true;
				BuyCommand.RaiseCanExecuteChanged();
			}
		}

		private bool IsBuyAllowed()
		{
			return isBuyAllowed;
		}

		#endregion

		private void LoadManifestData()
		{
			bool loadFailed = false;
			try
			{
				// get the data from the manifest file
				ApplicationTitle = ManifestAppInfo.Title;
				ApplicationTitleUpper = ApplicationTitle != null ? ApplicationTitle.ToUpperInvariant() : null;
				ApplicationProductId = ManifestAppInfo.ProductId;
				ApplicationDescription = ManifestAppInfo.Description;
				ApplicationVersion = ManifestAppInfo.Version;
				ApplicationAuthor = ManifestAppInfo.Author;
				ApplicationPublisher = ManifestAppInfo.Publisher;
			}
			catch (Exception ex)
			{
				ErrorDialogHelper.Show("An error occurred while loading the basic application information: " + ex.Message, "Error");
			}
		}

		private void LoadDesignData()
		{
			// get the data from the manifest file
			ApplicationTitle = "Application Title";
			ApplicationTitleUpper = "APPLICATION TITLE";
			ApplicationProductId = "Application.Id";
			ApplicationDescription = "Application Description Lorem Ipsum";
			ApplicationVersion = "AppVersion";
			ApplicationAuthor = "Application Author";
			ApplicationPublisher = "Application Publisher";

			MainItemData = new MainItemData();
			MainItemData.AppAuthorLabel = "Application Author";
			MainItemData.AppPublisherLabel = "Publisher";

			RaisePropertyChanged("MainItemData");
		}

		#region Processing of Data.xml

		private const string DataUri = @"Content\About\Data.xml";
		private const string ElementApp = "App";
		private const string ElementItems = "Items";
		private const string ElementMainItem = "MainItem";
		private const string ElementLink = "Link";
		private const string AttributeTitle = "Title";
		private const string AttributeProductId = "ProductId";
		private const string AttributeFullVersionProductId = "FullVersionProductId";
		private const string AttributeAuthor = "Author";
		private const string AttributeDescription = "Description";
		private const string AttributePublisher = "Publisher";
		private const string AttributeVersion = "Version";
		private const string AttributeAdditionalNotes = "AdditionalNotes";
		private const string AttributeAppAuthorLabel = "AppAuthorLabel";
		private const string AttributeAppPublisherLabel = "AppPublisherLabel";
		private const string AttributeAppDescriptionLabel = "AppDescriptionLabel";
		private const string AttributeAppVersionLabel = "AppVersionLabel";
		private const string AttributeAppAdditionalNotesLabel = "AppAdditionalNotesLabel";
		private const string AttributeAppReviewButtonContent = "AppReviewButtonContent";
		private const string AttributeAppBuyButtonContent = "AppBuyButtonContent";
		private const string AttributeContent = "Content";
		private const string AttributeNavigateUri = "NavigateUri";
		private const string AttributeLabel = "Label";
		private const string AttributeType = "Type";
		private const string AttributeUri = "Uri";

		public async Task LoadAboutData()
		{
			try
			{
				var path = Package.Current.InstalledLocation;
				var sri = await path.GetFileAsync(DataUri);
				
				var text = await FileIO.ReadTextAsync(sri);
				LoadFromStreamResourceInfo(text);
				// TODO: Localization?
			}
			catch (Exception ex)
			{
				ErrorDialogHelper.Show("An error occurred while loading the extended application information: " + ex.Message);
			}
		}

		private void LoadFromStreamResourceInfo(string xml)
		{
			var doc = XDocument.Parse(xml);
			var root = doc.Root;

			if (root != null)
			{
				// load app data
				var appElement = root.Element(ElementApp);
				if (appElement != null)
				{
					LoadAppData(appElement);
				}

				// load all pages
				var pagesElements = root.Element(ElementItems);
				if (pagesElements != null)
				{
					var pageElements = pagesElements.Elements();
					foreach (var pageElement in pageElements)
					{
						if (pageElement.Name == ElementMainItem)
						{
							LoadMainItemData(pageElement);
						}
						else
						{
							var page = LoadItemData(pageElement);
							Items.Add(page);
						}
					}
				}
			}
		}

		private void LoadAppData(XElement appElement)
		{
			// this loads the basic application data.
			// any element that is present here will override what has been 
			// retrieved from the WMAppManifest file, except the "AdditionalNotes"
			// (there's no corresponding entry in the app manifest for this)
			ApplicationTitle = GetAttributeValue(appElement, AttributeTitle) ?? ApplicationTitle;
			ApplicationTitleUpper = ApplicationTitle != null ? ApplicationTitle.ToUpperInvariant() : null;
			ApplicationProductId = GetAttributeValue(appElement, AttributeProductId) ?? ApplicationProductId;
			ApplicationFullVersionProductId = GetAttributeValue(appElement, AttributeFullVersionProductId) ?? ApplicationProductId;
			ApplicationAuthor = GetAttributeValue(appElement, AttributeAuthor) ?? ApplicationAuthor;
			ApplicationDescription = GetAttributeValue(appElement, AttributeDescription) ?? ApplicationDescription;
			ApplicationPublisher = GetAttributeValue(appElement, AttributePublisher) ?? ApplicationPublisher;
			ApplicationVersion = GetAttributeValue(appElement, AttributeVersion) ?? ApplicationVersion;
			AdditionalNotes = GetAttributeValue(appElement, AttributeAdditionalNotes);
		}

		private void LoadMainItemData(XElement itemElement)
		{
			MainItemData = new MainItemData();

			// get data
			MainItemData.Title = GetAttributeValue(itemElement, AttributeTitle, "about");
			MainItemData.AppAuthorLabel = GetAttributeValue(itemElement, AttributeAppAuthorLabel, "by");
			MainItemData.AppPublisherLabel = GetAttributeValue(itemElement, AttributeAppPublisherLabel, "Publisher:");
			MainItemData.AppDescriptionLabel = GetAttributeValue(itemElement, AttributeAppDescriptionLabel, "Description:");
			MainItemData.AppVersionLabel = GetAttributeValue(itemElement, AttributeAppVersionLabel, "Version:");
			MainItemData.AppAdditionalNotesLabel = GetAttributeValue(itemElement, AttributeAppAdditionalNotesLabel, "Additional notes:");
			MainItemData.AppReviewButtonContent = GetAttributeValue(itemElement, AttributeAppReviewButtonContent) ?? "Review this app!";
			MainItemData.AppBuyButtonContent = GetAttributeValue(itemElement, AttributeAppBuyButtonContent) ?? "Buy this app!";

			RaisePropertyChanged("MainItemData");
			// load all links
			var linkElements = itemElement.Descendants(ElementLink);
			foreach (var linkElement in linkElements)
			{
				var link = LoadLinkData(linkElement);
				Links.Add(link);
			}
		}

		private static LinkData LoadLinkData(XElement linkElement)
		{
			var result = new LinkData();

			// get data
			result.Content = GetAttributeValue(linkElement, AttributeContent);
			result.NavigateUri = GetAttributeValue(linkElement, AttributeNavigateUri);
			result.Label = GetAttributeValue(linkElement, AttributeLabel);

			return result;
		}

		private static ItemData LoadItemData(XElement itemElement)
		{
			var result = new ItemData();

			// get data
			result.Title = GetAttributeValue(itemElement, AttributeTitle);
			var typeText = GetAttributeValue(itemElement, AttributeType);
			result.ItemType = (ItemType)Enum.Parse(typeof(ItemType), typeText, true);
			var url = GetAttributeValue(itemElement, AttributeUri);
			if (!string.IsNullOrEmpty(url))
			{
				result.Uri = new Uri(url, UriKind.RelativeOrAbsolute);
			}

			// get the (alternate) content
			var child = itemElement.Descendants().FirstOrDefault();
			if (child != null)
			{
				// we have XML content here 
				// => use ToString() here to preserve the content as XML (it's likely XAML)
				result.OfflineContent = child.ToString();
			}
			else
			{
				// no child elements => use the content as text
				result.OfflineContent = itemElement.Value;
			}

			return result;
		}

		private static string GetAttributeValue(XElement element, string name, string valueIfNullOrEmpty)
		{
			var value = GetAttributeValue(element, name);
			if (string.IsNullOrEmpty(value))
			{
				return valueIfNullOrEmpty;
			}

			return value;
		}

		private static string GetAttributeValue(XElement element, string name)
		{
			var attribute = element.Attribute(name);
			if (attribute == null)
			{
				return null;
			}
			else
			{
				var value = attribute.Value;
				if (value != null)
				{
					value = value.Replace("\\n", Environment.NewLine);
				}

				return value;
			}
		}
		
		#endregion
	}
}