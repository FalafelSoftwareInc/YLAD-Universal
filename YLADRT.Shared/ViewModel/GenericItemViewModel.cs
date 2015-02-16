using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml.Markup;
using YLADRT.Shared.ViewModel;
using YLADRT.WindowsPhone.Models;

namespace YLADRT.Shared.ViewModel
{
    /// <summary>
    /// A view model for the generic pivot item pages.
    /// </summary>
    public sealed class GenericItemViewModel : BaseViewModel
    {
        private readonly ItemData data;
        private object content;

		public GenericItemViewModel()
		{
			this.Content = "TEST";
		}

        /// <summary>
        /// Gets or sets the content of the pivot item. 
        /// Usually this will be some visual element, but it can also be pure text.
        /// </summary>
        public object Content
        {
            get
            {
                return content;
            }

            set
            {
                if (content != value)
                {
                    content = value;
					RaisePropertyChanged("Content");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericItemViewModel"/> class using the specified item data.
        /// </summary>
        /// <param name="data">The item data used to configure the view model.</param>
        public GenericItemViewModel(ItemData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

			SetContent(data.OfflineContent);
        }

        public async Task DownloadExternalContent()
        {
			// if the data has an uri defined, try to get the content from there
			if (data.Uri != null)
			{
				try
				{
					var client = new HttpClient();
					var result = await client.GetStringAsync(data.Uri);
					SetContent(result);
				}
				catch (Exception)
				{
					// in case of an error, set the alternate content
					SetContent(data.OfflineContent);
				}
			}
			else
			{
				// if no uri is defined, set the alternate content
				SetContent(data.OfflineContent);
			}
        }

        private void SetContent(string rawContent)
        {
            // make sure we do this in the UI thread
			//if (Deployment.Current.Dispatcher.CheckAccess())
			//{
                try
                {
                    if (data.ItemType == ItemType.Text)
                    {
                        // process the content as string
                        ProcessTextContent(rawContent);
                    }
                    else if (data.ItemType == ItemType.Xaml)
                    {
                        // try loading the content as xaml
                        try
                        {
                            var o = XamlReader.Load(rawContent);
                            if (o != null)
                            {
                                Content = o;
                            }
                            else
                            {
                                Content = rawContent;
                            }
                        }
                        catch (Exception)
                        {
                            // try again as text (fallback)
                            ProcessTextContent(rawContent);
                        }
                    }
                    else
                    {
                        // try to use anything that has no correct type set as text
                        ProcessTextContent(rawContent);
                    }
                }
                catch (Exception)
                {
                    // in case of an error on this level, simply use the content as raw string
                    Content = rawContent;
                }
			//}
			//else
			//{
			//	Deployment.Current.Dispatcher.BeginInvoke(() => SetContent(rawContent));
			//}
        }

        private void ProcessTextContent(string raw)
        {
            // make sure we don't have \r (yes, a bit hacky)
            // => if someone uses sole \r as line breaks, we will lose the line breaks here,
            //    but it is a simple way to make this work for both Windows (\r\n) and Unix (\n)
            //    style line breaks.
            raw = raw.Replace("\r", string.Empty);

            // split, and keep empty entries
            var parts = raw.Split(new[] { "\n" }, StringSplitOptions.None);

            // now remove the leading empty entries
            var contentFound = false;
            var result = new List<string>();
            foreach (var part in parts)
            {
                if (string.IsNullOrEmpty(part) && !contentFound)
                {
                    continue;
                }

                contentFound = true;

                // add part to result
                if (part != null)
                {
                    result.Add(part.Trim());
                }
                else
                {
                    result.Add(string.Empty);
                }
            }

            // set content
            Content = result.ToArray();
        }
    }
}
