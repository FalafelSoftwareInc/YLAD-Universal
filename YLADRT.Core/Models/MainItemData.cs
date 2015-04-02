namespace YLADRT.Core.Models
{
	/// <summary>
	/// A container class for all the information needed to create the main item.
	/// </summary>
	public sealed class MainItemData
	{
		/// <summary>
		/// Gets or sets the title of the main item.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the label that should be used for the author. Default is "by".
		/// </summary>
		public string AppAuthorLabel { get; set; }

		/// <summary>
		/// Gets or sets the label that should be used for the description. Default is "Description:".
		/// </summary>
		public string AppDescriptionLabel { get; set; }

		/// <summary>
		/// Gets or sets the label that should be used for the publisher. Default is "Publisher:".
		/// </summary>
		public string AppPublisherLabel { get; set; }

		/// <summary>
		/// Gets or sets the label that should be used for the version. Default is "Version:".
		/// </summary>
		public string AppVersionLabel { get; set; }

		/// <summary>
		/// Gets or sets the label that should be used for the additional notes. Default is "Additional notes:".
		/// </summary>
		public string AppAdditionalNotesLabel { get; set; }

		/// <summary>
		/// Gets or sets the content that should be used for the review button. Default is "Review this app!".
		/// </summary>
		public string AppReviewButtonContent { get; set; }

		/// <summary>
		/// Gets or sets the content that should be used for the buy button. Default is "Buy this app!".
		/// </summary>
		public string AppBuyButtonContent { get; set; }
	}
}