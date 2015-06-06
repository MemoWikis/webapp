using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs082
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `membership`
	                ADD COLUMN `BillingEmail` VARCHAR(255) NULL DEFAULT NULL AFTER `Id`;"
            ).ExecuteUpdate();
        } 
    }
}