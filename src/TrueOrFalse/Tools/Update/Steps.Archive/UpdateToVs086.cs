using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs086
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE `user_to_follower` (
	                `User_id` INT(11) NOT NULL,
	                `Follower_id` INT(11) NOT NULL,
	                INDEX `Follower_id` (`Follower_id`),
	                INDEX `User_id` (`User_id`),
	                CONSTRAINT `FKBFFF1C125FCC0E1C` FOREIGN KEY (`Follower_id`) REFERENCES `user` (`Id`),
	                CONSTRAINT `FKBFFF1C1281B215E3` FOREIGN KEY (`User_id`) REFERENCES `user` (`Id`)
                )
                COLLATE='utf8_general_ci'
                ENGINE=InnoDB
                ;"
            ).ExecuteUpdate();
        }
    }
}