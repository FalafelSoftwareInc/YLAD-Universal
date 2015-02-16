using System;

namespace YLADRT.WindowsPhone.Models
{
    /// <summary>
    /// A container class for all the information needed to create a pivot item.
    /// </summary>
    public sealed class ItemData
    {
        /// <summary>
        /// Gets or sets the title of the pivot item.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the uri where the content of the page should be downloaded from.
        /// If this is <c>null</c>, the <see cref="OfflineContent"/> is used.
        /// </summary>
        public Uri Uri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the content, used for the formatting/preparation of the page.
        /// </summary>
        public ItemType ItemType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the alternate content that is used when downloading the content from the remote
        /// <c>Uri</c> fails, or if no <c>Uri</c> is given at all. The <see cref="ItemType"/> property
        /// is respected for this content too.
        /// </summary>
        public string OfflineContent
        {
            get;
            set;
        }
    }
}