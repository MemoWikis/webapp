using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs160
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE `categoryview` (
	                `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                `UserAgent` VARCHAR(1000) NULL DEFAULT NULL,
	                `DateCreated` DATETIME NULL DEFAULT NULL,
	                `Category_id` INT(11) NULL DEFAULT NULL,
	                `User_id` INT(11) NULL DEFAULT NULL,
	                PRIMARY KEY (`Id`),
	                INDEX `Category_id` (`Category_id`),
	                INDEX `User_id` (`User_id`)
                )
                COLLATE='utf8_general_ci'
                ENGINE=InnoDb;"

            ).ExecuteUpdate();
        }
    }
}

