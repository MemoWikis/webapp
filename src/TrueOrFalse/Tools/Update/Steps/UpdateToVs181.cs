using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs181
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `questionset`
	                CHANGE COLUMN `Name` `Name` VARCHAR(255) NULL DEFAULT NULL AFTER `Id`; "
            ).ExecuteUpdate();
        }
    }
}