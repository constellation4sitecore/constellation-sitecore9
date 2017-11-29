using Sitecore.Data.Items;

namespace Constellation.Foundation.ModelMapping
{
	public static class ItemExtensions
	{
		public static T MapToNew<T>(this Item item)
			where T : new()
		{
			return ModelMapper.MapItemToNew<T>(item);
		}
	}
}
