using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs075
    {
        public static void Run()
        {
            ServiceLocator
                .Resolve<ISession>()
                .CreateSQLQuery(
                    @"CREATE TABLE IF NOT EXISTS `gameround` (
                          `Id` int(11) NOT NULL AUTO_INCREMENT,
                          `Status` int(11) DEFAULT NULL,
                          `Number` smallint(6) DEFAULT NULL,
                          `DateCreated` datetime DEFAULT NULL,
                          `DateModified` datetime DEFAULT NULL,
                          `StartTime` datetime DEFAULT NULL,
                          `EndTime` datetime DEFAULT NULL,
                          `Question_id` int(11) DEFAULT NULL,
                          `Set_id` int(11) DEFAULT NULL,
                          `Game_id` int(11) DEFAULT NULL,
                          PRIMARY KEY (`Id`),
                          KEY `Question_id` (`Question_id`),
                          KEY `Set_id` (`Set_id`),
                          KEY `Game_id` (`Game_id`)
                        ) ENGINE=MyISAM DEFAULT CHARSET=utf8;"
            ).ExecuteUpdate();
        } 
    }
}