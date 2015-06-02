using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs079
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE IF NOT EXISTS `membership` (
                      `Id` int(11) NOT NULL AUTO_INCREMENT,
                      `BillingAddress` varchar(255) DEFAULT NULL,
                      `Price` decimal(19,5) DEFAULT NULL,
                      `PriceCategory` int(11) DEFAULT NULL,
                      `PaymentReceipt` datetime DEFAULT NULL,
                      `PaymentAmount` decimal(19,5) DEFAULT NULL,
                      `PeriodStart` datetime DEFAULT NULL,
                      `PeriodEnd` datetime DEFAULT NULL,
                      `DateCreated` datetime DEFAULT NULL,
                      `DateModified` datetime DEFAULT NULL,
                      `User_id` int(11) DEFAULT NULL,
                      PRIMARY KEY (`Id`),
                      KEY `User_id` (`User_id`)
                    ) ENGINE=MyISAM DEFAULT CHARSET=utf8;"
            ).ExecuteUpdate();
        } 
    }
}