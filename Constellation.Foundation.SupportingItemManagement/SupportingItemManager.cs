using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Engines;
using Sitecore.Diagnostics;
using Sitecore.Install.Files;
using Sitecore.Install.Framework;
using Sitecore.Install.Items;
using Sitecore.Install.Utils;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Constellation.Foundation.SupportingItemManagement
{
	public class SupportingItemManager
	{
		#region Fields

		#endregion


		#region Main

		public bool EnsureDependentItems()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (var assembly in assemblies)
			{
				var types = GetLoadableTypes(assembly);

				foreach (var type in types)
				{
					if (!Attribute.IsDefined(type, typeof(HasItemDependenciesAttribute)))
					{
						continue;
					}

					if (!EnsureItemDependencies(type))
					{
						Log.Warn($"Item Dependency Operation Failed for Type {type.FullName}", this);
					}
				}
			}
		}


		private bool EnsureItemDependencies(Type type)
		{
			if (RequiredItemMissing())
			{
				var filepath = "";
				//if we have an absolute path, rather than relative to the site root
				if (System.Text.RegularExpressions.Regex.IsMatch(Settings.DataFolder, @"^(([a-zA-Z]:\\)|(//)).*"))
					filepath = Settings.DataFolder +
							   @"\packages\ThisIsMyPackage.zip";
				else
					filepath = State.HttpRuntime.AppDomainAppPath + Settings.DataFolder.Substring(1) +
							   @"\packages\ThisIsMyPackage.zip";
				try
				{
					type.Assembly
						.GetManifestResourceStream($"{type.Assembly.GetName()}.ThisIsMyPackage.zip")
						?.CopyTo(new FileStream(filepath, FileMode.Create));
					Task.Run(() =>
					{

						while (true)
						{
							if (!IsFileLocked(new FileInfo(filepath)))
							{

								using (new SecurityDisabler())
								{
									using (new SyncOperationContext())
									{
										IProcessingContext context = new SimpleProcessingContext();
										IItemInstallerEvents events =
											new DefaultItemInstallerEvents(
												new BehaviourOptions(InstallMode.Overwrite, MergeMode.Undefined));
										context.AddAspect(events);
										IFileInstallerEvents events1 = new DefaultFileInstallerEvents(true);
										context.AddAspect(events1);

										Sitecore.Install.Installer installer = new Sitecore.Install.Installer();
										installer.InstallPackage(MainUtil.MapPath(filepath), context);
										break;
									}
								}
							}

							Thread.Sleep(1000);
						}
					});
				}
				catch (Exception e)
				{
					Log.Error("Unable to install dependent package", e, this);
					return false;
				}

				return true;
			}

			return true;
		}

		private bool RequiredItemMissing()
		{

		}
		#endregion

		/// <summary>
		/// checks to see if the file is done being written to the filesystem
		/// </summary>


		/// from http://stackoverflow.com/questions/29359132/reading-file-that-might-be-updating
		/// <param name="file"></param>
		/// <returns></returns>
		protected virtual bool IsFileLocked(FileInfo file)
		{
			FileStream stream = null;

			try
			{
				stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
			}
			catch (IOException)
			{
				//the file is unavailable because it is:
				//still being written to
				//or being processed by another thread
				//or does not exist (has already been processed)
				return true;
			}
			finally
			{
				if (stream != null)
					stream.Close();
			}

			//file is not locked
			return false;
		}

		/// <summary>
		/// Inspects the provided assembly and returns only a list of types that are capable
		/// of being loaded by the current application.
		/// </summary>
		/// <remarks>
		/// See http://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx for 
		/// details on why this is required.
		/// </remarks>
		/// <param name="assembly">The assembly to parse.</param>
		/// <returns>A list of loadable types.</returns>
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Stylecop hates URLs.")]
		private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
		{
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				return e.Types.Where(t => t != null);
			}
		}
	}
}
