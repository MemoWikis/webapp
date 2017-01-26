using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs164
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE `messageemail` (
	                `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                `User_id` INT(11) NULL DEFAULT NULL,
	                `MessageEmailType` INT(11) NULL DEFAULT NULL,
	                `DateCreated` DATETIME NULL DEFAULT NULL,
	                `DateModified` DATETIME NULL DEFAULT NULL,
	                PRIMARY KEY (`Id`),
	                INDEX `User_id` (`User_id`)
                )
                COLLATE='utf8_general_ci'
                ENGINE=MEMORY;"
            ).ExecuteUpdate();
        }
    }
}