using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs108
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `question`
	                ADD COLUMN `License` VARCHAR(1000) NULL DEFAULT NULL AFTER `Description`;"
            ).ExecuteUpdate();
        }
    }
}