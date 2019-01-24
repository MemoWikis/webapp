using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs198
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"CREATE TABLE `questionchange` (
	                `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                `Data` TEXT NULL DEFAULT NULL,
	                `Type` INT NULL,
	                `DataVersion` INT(11) NULL DEFAULT NULL,
	                `DateCreated` DATETIME NULL DEFAULT NULL,
	                `Author_Id` INT(11) NULL DEFAULT NULL,
	                `Question_Id` INT NULL DEFAULT NULL,
	                PRIMARY KEY (`Id`),
	                INDEX `FK_Question_Author_Id` (`Author_Id`),
	                INDEX `FK_Question_Id` (`Question_Id`),
	                CONSTRAINT `FK_Question_Author_Id` FOREIGN KEY (`Author_Id`) REFERENCES `user` (`Id`)
                  )
                  COLLATE='utf8_general_ci'
                  ENGINE=InnoDB;"
                ).ExecuteUpdate();
        }
    }
}