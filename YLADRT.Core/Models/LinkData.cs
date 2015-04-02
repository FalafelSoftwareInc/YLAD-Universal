using System.Diagnostics.CodeAnalysis;

namespace YLADRT.Core.Models
{
	/// <summary>
	/// A container class for all the information required to build a hyperlink button element.
	/// </summary>
	public sealed class LinkData
	{
		private static LinkData _empty;

		/// <summary>
		/// Gets an empty link data object.
		/// </summary>
		public static LinkData Empty
		{
			get
			{
				if (_empty == null)
				{
					_empty = new LinkData();
				}

				return _empty;
			}
		}

		/// <summary>
		/// Gets or sets the uri used as navigation target.
		/// </summary>
		[SuppressMessage("Microsoft.Design",
			"CA1056:UriPropertiesShouldNotBeStrings",
			Justification = "More convenient, converted to Uri by the binding engine.")]
		public string NavigateUri { get; set; }

		/// <summary>
		/// Gets or sets the content used as display.
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// Gets or sets an additional label that is not part of the hyperlink button.
		/// </summary>
		public string Label { get; set; }
	}
}