namespace Constellation.Foundation.SupportingItemManagement
{
	public abstract class ItemDependencyCheck
	{
		public bool CheckDependencies()
		{
			if (!DependenciesExist())
			{
				return false;
			}

			if (!DependenciesHaveValidValues())
			{
				return false;
			}

			if (!DependenciesHaveValidSecurity())
			{
				return false;
			}

			return true;
		}

		protected abstract bool DependenciesExist();

		protected virtual bool DependenciesHaveValidValues()
		{
			return true;
		}

		protected virtual bool DependenciesHaveValidSecurity()
		{
			return true;
		}
	}
}
