using Sitecore.Data;
using Sitecore.Globalization;

namespace Constellation.Foundation.Repositories
{
	interface IRepository
	{
		Database Database { get; }

		Language Language { get; }
	}
}
