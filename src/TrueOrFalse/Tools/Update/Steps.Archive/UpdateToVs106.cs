using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs106
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(@"
                CREATE TABLE `badge` (
	                `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                `BadgeTypeKey` VARCHAR(1000) NULL DEFAULT NULL,
	                `TimesGiven` INT(11) NULL DEFAULT NULL,
	                `Points` INT(11) NULL DEFAULT NULL,
	                `Level` VARCHAR(1000) NULL DEFAULT NULL,
	                `DateCreated` DATETIME NULL DEFAULT NULL,
	                `DateModified` DATETIME NULL DEFAULT NULL,
	                `User_id` INT(11) NULL DEFAULT NULL,
	                PRIMARY KEY (`Id`),
	                INDEX `User_id` (`User_id`)
                )
                COLLATE='utf8_general_ci'
                ENGINE=InnoDb;"
            ).ExecuteUpdate();
        }
    }
}