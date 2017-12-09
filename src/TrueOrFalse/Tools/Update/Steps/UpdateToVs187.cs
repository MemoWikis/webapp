using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs187
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `categorychange`
	                CHANGE COLUMN `Data` `Data` TEXT NULL DEFAULT NULL AFTER `Id`;"
            ).ExecuteUpdate();
        }
    }
}

