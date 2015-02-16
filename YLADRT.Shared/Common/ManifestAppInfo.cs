using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Storage;

namespace YLADRT.Shared.Common
{
    /// <summary>
    /// Extracts the information contained in the AppxManifest.xml file.
    /// Extended and improved version based on a post by Joost van Schaik:
    /// http://dotnetbyexample.blogspot.com/2011/03/easy-access-to-wmappmanifestxml-app.html
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "These are valid and correctly spelled names and urls.")]
    internal static class ManifestAppInfo
    {
        private const string VersionLabel = "VERSION";
        private const string ProductIdLabel = "PRODUCTID";
        private const string TitleLabel = "TITLE";
        private const string GenreLabel = "GENRE";
        private const string DescriptionLabel = "DESCRIPTION";
        private const string AuthorLabel = "AUTHOR";
        private const string PublisherLabel = "PUBLISHER";

        private static Dictionary<string, string> attributes;

        private static Dictionary<string, string> Attributes
        {
            get
            {
                return attributes;
            }
        }

        static ManifestAppInfo()
        {
			// add a bit of design-time data
			if (DesignMode.DesignModeEnabled)
			{
				LoadDesignTimeData();
			}
			else
			{
				LoadData();
			}
        }

        private static void LoadDesignTimeData()
        {
            attributes = new Dictionary<string, string>
                              {
                                  {
                                      VersionLabel, "1.2.3.4"
                                  }, 
                                  {
                                      ProductIdLabel, "{CF68A1E0-578C-4A7C-9278-6AC10F51EAE1}"
                                  }, 
                                  {
                                      TitleLabel, "My Title"
                                  }, 
                                  {
                                      GenreLabel, "apps.normal"
                                  }, 
                                  {
                                      DescriptionLabel, "Some really long sample description that exceeds the available width of the screen to test whether wrapping works correctly :-)."
                                  }, 
                                  {
                                      AuthorLabel, "Mr. Author"
                                  }, 
                                  {
                                      PublisherLabel, "My Publisher"
                                  }
                              };
        }

        private static void LoadData()
        {
            // create the dictionary and parse the xml file
			attributes = new Dictionary<string, string>();

			try
			{
				// load the manifest file
				var doc = XDocument.Load("AppxManifest.xml", LoadOptions.None);

				// Define the default namespaces to be used
				var xname = XNamespace.Get("http://schemas.microsoft.com/appx/2010/manifest");
				var mp = XNamespace.Get("http://schemas.microsoft.com/appx/2014/phone/manifest");
				var m3 = XNamespace.Get("http://schemas.microsoft.com/appx/2014/manifest");

				// extract data from the various elements in the xml
				var packageElement = doc.Descendants(xname + "Package").FirstOrDefault();
				if (packageElement == null) return;

				var identityElement = packageElement.Descendants(xname + "Identity").FirstOrDefault();
				if (identityElement != null)
				{
					var attribute = identityElement.Attributes("Version").FirstOrDefault();
					if (attribute != null)
					{
						attributes.Add(VersionLabel, attribute.Value);
					}
				}

				var phoneIdentityElement = packageElement.Descendants(mp + "PhoneIdentity").FirstOrDefault();
				if (phoneIdentityElement != null)
				{
					var productID = phoneIdentityElement.Attributes("PhoneProductId").FirstOrDefault();
					if (productID != null)
					{
						attributes.Add(ProductIdLabel, productID.Value);
					}
				}

				var propElement = packageElement.Descendants(xname + "Properties").FirstOrDefault();
				if (propElement == null) return;

				var displayNameElement = propElement.Descendants(xname + "DisplayName").FirstOrDefault();
				if (displayNameElement != null)
				{
					attributes.Add(TitleLabel, displayNameElement.Value);
				}

				var publisherNameElement = propElement.Descendants(xname + "PublisherDisplayName").FirstOrDefault();
				if (publisherNameElement != null)
				{
					attributes.Add(PublisherLabel, publisherNameElement.Value);
					attributes.Add(AuthorLabel, publisherNameElement.Value);
				}

				var appsElement = packageElement.Descendants(xname + "Applications").FirstOrDefault();
				if (appsElement == null) return;

				var appElement = appsElement.Descendants(xname + "Application").FirstOrDefault();
				if (appsElement == null) return;

				var visElement = appElement.Descendants(m3 + "VisualElements").FirstOrDefault();
				if (visElement != null)
				{
					// get all attributes
					var descriptionAttribute = visElement.Attributes("Description").FirstOrDefault();
					if (descriptionAttribute != null)
					{
						attributes.Add(DescriptionLabel, descriptionAttribute.Value);
					}
				}
			}
			catch (Exception ex)
			{
				// ignore
			}
        }

        /// <summary>
        /// Gets the version string in the AppxManifest.xml or <c>null</c> if this information could not be retrieved.
        /// </summary>
        public static string Version
        {
            get
            {
                return Attributes.ContainsKey(VersionLabel) ? Attributes[VersionLabel] : null;
            }
        }

        /// <summary>
        /// Gets the product Id in the AppxManifest.xml or <c>null</c> if this information could not be retrieved.
        /// </summary>
        public static string ProductId
        {
            get
            {
                return Attributes.ContainsKey(ProductIdLabel) ? Attributes[ProductIdLabel] : null;
            }
        }

        /// <summary>
        /// Gets the title in the AppxManifest.xml or <c>null</c> if this information could not be retrieved.
        /// </summary>
        public static string Title
        {
            get
            {
                return Attributes.ContainsKey(TitleLabel) ? Attributes[TitleLabel] : null;
            }
        }

        /// <summary>
        /// Gets the genre in the AppxManifest.xml or <c>null</c> if this information could not be retrieved.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", 
            "CA1811:AvoidUncalledPrivateCode", 
            Justification = "Not used at the moment, but may be in the future.")]
        public static string Genre
        {
            get
            {
                return Attributes.ContainsKey(GenreLabel) ? Attributes[GenreLabel] : null;
            }
        }

        /// <summary>
        /// Gets the description in the AppxManifest.xml or <c>null</c> if this information could not be retrieved.
        /// </summary>
        public static string Description
        {
            get
            {
                return Attributes.ContainsKey(DescriptionLabel) ? Attributes[DescriptionLabel] : null;
            }
        }

        /// <summary>
        /// Gets the author in the AppxManifest.xml or <c>null</c> if this information could not be retrieved.
        /// </summary>
        public static string Author
        {
            get
            {
                return Attributes.ContainsKey(AuthorLabel) ? Attributes[AuthorLabel] : null;
            }
        }

        /// <summary>
        /// Gets the publisher in the AppxManifest.xml or <c>null</c> if this information could not be retrieved.
        /// </summary>
        public static string Publisher
        {
            get
            {
                return Attributes.ContainsKey(PublisherLabel) ? Attributes[PublisherLabel] : null;
            }
        }
    }
}
