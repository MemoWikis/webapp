using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs186
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE `categorychange` (
                  	`Id` INT(11) NOT NULL AUTO_INCREMENT,
                  	`Data` VARCHAR(1000) NULL DEFAULT NULL,
                  	`DataVersion` INT(11) NULL DEFAULT NULL,
                  	`DateCreated` DATETIME NULL DEFAULT NULL,
                  	`Author_id` INT(11) NULL DEFAULT NULL,
                  	PRIMARY KEY (`Id`),
                  	INDEX `FK_Author_id` (`Author_id`),
                  	CONSTRAINT `FK_Author_id` FOREIGN KEY (`Author_id`) REFERENCES `user` (`Id`)
                  )
                  COLLATE='utf8_general_ci'
                  ENGINE=InnoDB;"
            ).ExecuteUpdate();
        }
    }
}

