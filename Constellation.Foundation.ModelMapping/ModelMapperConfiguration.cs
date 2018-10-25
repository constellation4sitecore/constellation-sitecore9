using System;
using System.Collections.Generic;
using System.Xml;
using Constellation.Foundation.ModelMapping.FieldMappers;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.ModelMapping
{
	/// <summary>
	/// The Runtime settings for ModelMapper.
	/// </summary>
	public class ModelMapperConfiguration
	{
		#region Locals
		private static volatile ModelMapperConfiguration _current;

		private static readonly object LockObject = new object();
		#endregion

		/// <summary>
		/// Creates a new instance of ModelMapperConfiguration
		/// </summary>
		protected ModelMapperConfiguration()
		{
			FieldMapperTypes = new SortedDictionary<string, ICollection<Type>>();
		}

		#region Properties

		/// <summary>
		/// The current runtime settings for ModelMapper.
		/// </summary>
		public static ModelMapperConfiguration Current
		{
			get
			{
				if (_current == null)
				{
					lock (LockObject)
					{
						if (_current == null)
						{
							_current = CreateNewConfiguration();
						}
					}
				}

				return _current;
			}
		}

		/// <summary>
		/// The list of all FieldMapper Types.
		/// </summary>
		protected SortedDictionary<string, ICollection<Type>> FieldMapperTypes { get; }

		/// <summary>
		/// The Type to use when a given Sitecore field type does not have an explicit Mapper set.
		/// </summary>
		public Type DefaultFieldMapperType { get; private set; }

		/// <summary>
		/// Whether the Mapper should gracefully "eat" errors or fail hard. We recommend setting this to true in production.
		/// </summary>
		public bool ContinueOnError { get; private set; }

		/// <summary>
		/// Whether the Mapper should attempt to map Sitecore's "Standard" template fields. This should normally be true.
		/// </summary>
		public bool IgnoreStandardFields { get; private set; }
		#endregion

		#region Methods

		/// <summary>
		/// For a given Sitecore Field Type, return the Mapper types to process the field value.
		/// </summary>
		/// <param name="type">The Sitecore Field Type string.</param>
		/// <returns>A colleciton of Mapper types to instatiate.</returns>
		public ICollection<Type> GetMappersForFieldType(string type)
		{
			ICollection<Type> types = new List<Type> { DefaultFieldMapperType };

			if (FieldMapperTypes.ContainsKey(type))
			{
				types = FieldMapperTypes[type];
			}

			return types;
		}


		#endregion


		private static ModelMapperConfiguration CreateNewConfiguration()
		{
			var output = new ModelMapperConfiguration();

			var libraryNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/modelMapping");

			output.DefaultFieldMapperType = libraryNode?.Attributes?["defaultFieldMapper"]?.Value == null
				? typeof(TextFieldMapper)
				: Type.GetType(libraryNode.Attributes["defaultFieldMapper"].Value);

			var continueString = libraryNode?.Attributes?["continueOnError"].Value;

			if (string.IsNullOrEmpty(continueString))
			{
				continueString = "true";
			}

			if (bool.TryParse(continueString, out var continueResult))
			{
				output.ContinueOnError = continueResult;
			}
			else
			{
				throw new Exception("Configuration Error. Constellation.Foundation.ModelMapping.config contains a non-boolean value for the \"continueOnError\" Attribute. Valid values are \"true\" or \"false\"");
			}

			var ignoreString = libraryNode?.Attributes?["ignoreStandardFields"]?.Value;

			if (string.IsNullOrEmpty(ignoreString))
			{
				ignoreString = "true";
			}

			if (bool.TryParse(ignoreString, out var ignoreResult))
			{
				output.IgnoreStandardFields = ignoreResult;
			}
			else
			{
				throw new Exception("Configuration Error. Constellation.Foundation.ModelMapping.config contains a non-boolean value for the \"continueOnError\" Attribute. Valid values are \"true\" or \"false\"");
			}


			if (libraryNode == null || !libraryNode.HasChildNodes) return output;

			var fields = libraryNode.ChildNodes;

			foreach (XmlNode fieldNode in fields)
			{
				var fieldTypeName = fieldNode?.Attributes?["type"]?.Value;

				if (string.IsNullOrEmpty(fieldTypeName))
				{
					continue;
				}

				ICollection<Type> mappers;

				// create or retrieve the field's list of mappers from the dictionary
				if (output.FieldMapperTypes.ContainsKey(fieldTypeName))
				{
					mappers = output.FieldMapperTypes[fieldTypeName];
				}
				else
				{
					mappers = new List<Type>();
					output.FieldMapperTypes.Add(fieldTypeName, mappers);
				}

				// Add types to process for the given field.
				var mapperNodes = fieldNode.ChildNodes;

				foreach (XmlNode mapperNode in mapperNodes)
				{
					var typeName = mapperNode?.Attributes?["type"]?.Value;

					if (string.IsNullOrEmpty(typeName))
					{
						continue;
					}

					try
					{
						var mapperType = Type.GetType(typeName);

						if (mapperType == null)
						{
							var ex = new Exception($"Configuration error! The Field Mapper Type: {typeName} could not be located. Check your spelling and assemblies.");

							Log.Error(ex.Message, ex, output);
							throw ex;
						}


						if (!typeof(IFieldMapper).IsAssignableFrom(mapperType))
						{
							var ex = new Exception($"Configuration error! The Field Mapper Type: {typeName} does not descend from IFieldMapper and cannot be used.");

							Log.Error(ex.Message, ex, output);
							throw ex;
						}

						mappers.Add(mapperType);
					}
					catch (Exception ex)
					{
						Log.Error($"Configuration Error! Failure attempting to load Field Mapper Type: {typeName}", ex, output);
						throw ex;
					}
				}
			}

			return output;
		}
	}
}
