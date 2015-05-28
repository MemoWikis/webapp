using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs077
    {
        public static void Run()
        {
            ServiceLocator
                .Resolve<ISession>()
                .CreateSQLQuery(
                      @"CREATE TABLE IF NOT EXISTS `game_player` (
                            `Id` int(11) NOT NULL AUTO_INCREMENT,
                            `Position` int(11) DEFAULT NULL,
                            `IsCreator` tinyint(1) DEFAULT NULL,
                            `DateCreated` datetime DEFAULT NULL,
                            `DateModified` datetime DEFAULT NULL,
                            `Game_id` int(11) DEFAULT NULL,
                            `User_id` int(11) DEFAULT NULL,
                            PRIMARY KEY (`Id`),
                            KEY `Game_id` (`Game_id`),
                            KEY `User_id` (`User_id`)
                        ) ENGINE=MyISAM DEFAULT CHARSET=utf8;"
            ).ExecuteUpdate();
        } 
    }
}