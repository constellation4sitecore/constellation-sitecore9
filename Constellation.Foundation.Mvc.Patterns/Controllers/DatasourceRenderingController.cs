﻿using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;

namespace Constellation.Foundation.Mvc.Patterns.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// A controller for renderings where the Rendering has an explicit datasource Item or explicitly uses the
	/// Context Item where the facts needed by the view are (generally) available via the fields of this Item.
	/// The Controller uses ModelMapper to map the Item to a ViewModel which is then handed to the View.
	/// </summary>
	/// <typeparam name="TModel">A class that can be mapped from an Item using ModelMapper.</typeparam>
	public abstract class DatasourceRenderingController<TModel> : ConventionController
	where TModel : class, new()
	{
		/// <summary>
		/// Creates a new instance of DatasourceRenderingController
		/// </summary>
		/// <param name="viewPathResolver">The view path resolver.</param>
		/// <param name="modelMapper">An instance of ModelMapper, typically provided by Dependency Injection.</param>
		protected DatasourceRenderingController(IViewPathResolver viewPathResolver, IModelMapper modelMapper) : base(viewPathResolver)
		{
			ModelMapper = modelMapper;
		}

		#region Properties
		/// <summary>
		/// The instance of ModelMapper to use for mapping Items.
		/// </summary>
		protected IModelMapper ModelMapper { get; }
		#endregion

		/// <inheritdoc />
		protected override object GetModel(Item datasource, Item contextItem)
		{
			return ModelMapper.MapItemToNew<TModel>(datasource);
		}
	}
}
