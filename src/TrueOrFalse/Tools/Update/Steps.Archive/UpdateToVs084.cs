using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs084
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE `date` (
	                  `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                  `Visibility` INT(11) NULL DEFAULT NULL,
	                  `Details` VARCHAR(255) NULL DEFAULT NULL,
	                  `DateTime` DATETIME NULL DEFAULT NULL,
	                  `DateCreated` DATETIME NULL DEFAULT NULL,
	                  `DateModified` DATETIME NULL DEFAULT NULL,
	                  PRIMARY KEY (`Id`)
                  )
                  COLLATE='utf8_general_ci'
                  ENGINE=InnoDB;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE `date_to_sets` (
	                  `Date_id` INT(11) NOT NULL,
	                  `Set_id` INT(11) NOT NULL,
	                  INDEX `Set_id` (`Set_id`),
	                  INDEX `Date_id` (`Date_id`),
	                  CONSTRAINT `FKCB583B0E93032C55` FOREIGN KEY (`Set_id`) REFERENCES `questionset` (`Id`),
	                  CONSTRAINT `FKCB583B0EBC787FC9` FOREIGN KEY (`Date_id`) REFERENCES `date` (`Id`)
                  )
                  COLLATE='utf8_general_ci'
                  ENGINE=InnoDB;"
            ).ExecuteUpdate();
        }
    }
}