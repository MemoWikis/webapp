using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs031
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `question`
	                ADD COLUMN `SetsAmount` INT NULL AFTER `TotalViews`,
	                ADD COLUMN `SetsTop5Json` VARCHAR(1000) NULL AFTER `SetsAmount`;").ExecuteUpdate();
        }
    }
}
