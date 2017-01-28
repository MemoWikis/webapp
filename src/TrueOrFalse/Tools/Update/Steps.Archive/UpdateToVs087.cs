using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs087
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE `appaccess` (
	                `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                `AppKey` INT(11) NULL DEFAULT NULL,
	                `AccessToken` VARCHAR(255) NULL DEFAULT NULL,
	                `DateCreated` DATETIME NULL DEFAULT NULL,
	                `DateModified` DATETIME NULL DEFAULT NULL,
	                `User_id` INT(11) NULL DEFAULT NULL,
	                PRIMARY KEY (`Id`),
	                INDEX `User_id` (`User_id`),
	                CONSTRAINT `FKB4BC77D581B215E3` FOREIGN KEY (`User_id`) REFERENCES `user` (`Id`)
                )
                COLLATE='utf8_general_ci'
                ENGINE=InnoDB
                AUTO_INCREMENT=2;"
            ).ExecuteUpdate();
        }
    }
}