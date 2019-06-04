using System;
using System.IO;
using System.Reflection;
using System.Web;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Given an Image field, renders the associated SVG file as a string to a Model Property suffixed with the word "Svg".
	/// </summary>
	public class ImageSvgMapper : FieldAttributeMapper
	{
		/// <summary>
		/// Attempts to map a specific Attribute of a Sitecore XML Field to a specific Property of the supplied modelInstance.
		/// </summary>
		/// <param name="modelInstance">The model to inspect.</param>
		/// <param name="field">The field to inspect.</param>
		/// <returns>A MappingStatus indicating whether the mapping was successful and other useful performance facts.</returns>
		public override FieldMapStatus Map(object modelInstance, Field field)
		{
			Model = modelInstance;
			Field = field;

			var name = GetPropertyName();

			Property = modelInstance.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public);

			if (Property == null)
			{
				return FieldMapStatus.NoProperty;
			}

			try
			{
				var value = (string)GetValueToAssign();

				if (string.IsNullOrEmpty(value))
				{
					return FieldMapStatus.ValueEmpty;
				}

				if (Property.Is<HtmlString>())
				{
					Property.SetValue(Model, new HtmlString(value));
					return FieldMapStatus.Success;
				}

				if (Property.Is<string>())
				{
					Property.SetValue(Model, value);
					return FieldMapStatus.Success;
				}

				return FieldMapStatus.TypeMismatch;
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex, this);
				return FieldMapStatus.Exception;
			}
		}


		/// <inheritdoc />
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Svg";
		}

		/// <inheritdoc />
		protected override object GetValueToAssign()
		{
			return RenderSvgContents();
		}

		private string RenderSvgContents()
		{
			ImageField field = this.Field;

			if (field.MediaID.IsGlobalNullId)
			{
				Log.Debug("ImageSvgMapper: MediaID was null.");
				return string.Empty;
			}

			if (field.MediaItem == null)
			{
				Log.Debug("ImageSvgMapper: MediaItem was null.");
				return string.Empty;
			}

			var mediaItem = new MediaItem(field.MediaItem);

			if (mediaItem.MimeType != "image/svg+xml")
			{
				Log.Debug("ImageSvgMapper: MimeType was not image/svg+xml.");
				return string.Empty;
			}

			using (var reader = new StreamReader(mediaItem.GetMediaStream()))
			{
				Log.Debug("ImageSvgMapper: reading Media Stream to Model.");
				return reader.ReadToEnd();
			}
		}
	}
}
