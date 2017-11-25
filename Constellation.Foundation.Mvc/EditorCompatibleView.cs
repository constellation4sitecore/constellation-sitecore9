namespace Constellation.Foundation.Mvc
{
	/// <inheritdoc />
	/// <summary>
	/// An implementation of System.Web.Mvc.WebViewPage that includes a few
	/// Sitecore context properties useful for View-specific logic.
	/// </summary>
	/// <remarks>
	/// While some MVC strategies on Sitecore call for separate Views depending
	/// upon the current Editing Context state, it may be more efficient and 
	/// easy to understand if one simply has the View decide what to render 
	/// based upon the Editing state. (This is 100% valid View strategy, since
	/// the Model shouldn't vary, just the markup)
	/// </remarks>
	public abstract class EditorCompatibleView : System.Web.Mvc.WebViewPage
	{
		/// <summary>
		/// Gets a value indicating that the page is open in the Experience Editor.
		/// </summary>
		protected bool InEditor
		{
			get
			{
				return Sitecore.Context.PageMode.IsExperienceEditor;
			}
		}

		/// <summary>
		/// Gets a value indicating that the page is being edited in the Experience Editor.
		/// </summary>
		protected bool IsEditing
		{
			get
			{
				return Sitecore.Context.PageMode.IsExperienceEditorEditing;
			}
		}
	}

	/// <inheritdoc />
	/// <summary>
	/// An implementation of System.Web.Mvc.WebViewPage`1[] that includes a few
	/// Sitecore context properties useful for View-specific logic.
	/// </summary>
	/// <remarks>
	/// While some MVC strategies on Sitecore call for separate Views depending
	/// upon the current Editing Context state, it may be more efficient and 
	/// easy to understand if one simply has the View decide what to render 
	/// based upon the Editing state. (This is 100% valid View strategy, since
	/// the Model shouldn't vary, just the markup)
	/// </remarks>
	public abstract class EditorCompatibleView<TModel> : System.Web.Mvc.WebViewPage<TModel>
	{
		/// <summary>
		/// Gets a value indicating that the page is open in the Experience Editor.
		/// </summary>
		protected bool InEditor
		{
			get
			{
				return Sitecore.Context.PageMode.IsExperienceEditor;
			}
		}

		/// <summary>
		/// Gets a value indicating that the page is being edited in the Experience Editor.
		/// </summary>
		protected bool IsEditing
		{
			get
			{
				return Sitecore.Context.PageMode.IsExperienceEditorEditing;
			}
		}
	}
}