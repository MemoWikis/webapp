using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs080
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `membership`
	                CHANGE COLUMN `Price` `PricePerMonth` DECIMAL(19,5) NULL DEFAULT NULL AFTER `BillingAddress`;"
            ).ExecuteUpdate();
        } 
    }
}