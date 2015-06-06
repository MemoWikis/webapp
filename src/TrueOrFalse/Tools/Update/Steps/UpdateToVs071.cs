using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs071
    {
        public static void Run()
        {
            ServiceLocator
                .Resolve<ISession>()
                .CreateSQLQuery(
                @"  CREATE TABLE IF NOT EXISTS `game` (
                      `Id` int(11) NOT NULL AUTO_INCREMENT,
                      `WillStartAt` datetime DEFAULT NULL,
                      `Status` int(11) DEFAULT NULL,
                      `Comment` varchar(255) DEFAULT NULL,
                      `DateCreated` datetime DEFAULT NULL,
                      `DateModified` datetime DEFAULT NULL,
                      `Creator_id` int(11) DEFAULT NULL,
                      PRIMARY KEY (`Id`),
                      KEY `Creator_id` (`Creator_id`)
                    ) ENGINE=MyISAM DEFAULT CHARSET=utf8;

                    CREATE TABLE IF NOT EXISTS `games_to_sets` (
                      `Game_id` int(11) NOT NULL,
                      `Set_id` int(11) NOT NULL,
                      KEY `Set_id` (`Set_id`),
                      KEY `Game_id` (`Game_id`)
                    ) ENGINE=MyISAM DEFAULT CHARSET=utf8;

                    CREATE TABLE IF NOT EXISTS `games_to_users` (
                      `Game_id` int(11) NOT NULL,
                      `User_id` int(11) NOT NULL,
                      KEY `User_id` (`User_id`),
                      KEY `Game_id` (`Game_id`)
                    ) ENGINE=MyISAM DEFAULT CHARSET=utf8;"
            ).ExecuteUpdate();
        } 
    }
}