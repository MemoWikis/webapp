using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs081
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `membership`
	                ADD COLUMN `BillingName` VARCHAR(255) NULL DEFAULT NULL AFTER `Id`;"
            ).ExecuteUpdate();
        } 
    }
}