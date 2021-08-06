using Constellation.Foundation.Mvc;

namespace Constellation.Foundation.Labels
{
	/// <summary>
	/// An implementation of System.Web.Mvc.WebViewPage that includes a few
	/// Sitecore context properties useful for View-specific logic.
	/// </summary>
	/// <typeparam name="TModel">The Type of the Model that the view will operate against</typeparam>
	/// <typeparam name="TLabels">The Type of Label object that the view needs to provide for translatable labels.</typeparam>
	public abstract class EditorCompatibleView<TModel, TLabels> : EditorCompatibleView<TModel>
		where TLabels : class, new()
	{
		private TLabels _labels = null;

		/// <summary>
		/// Gets the Labels object associated with this View.
		/// </summary>
		public TLabels Labels
		{
			get
			{
				if (_labels == null)
				{
					_labels = LabelRepository.GetLabelsForView<TLabels>();
				}

				return _labels;
			}
		}


	}
}
