using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs128
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"
                    CREATE TABLE `runningjob` (
	                    `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                    `StartAt` DATETIME NULL DEFAULT NULL,
	                    `Name` VARCHAR(1000) NULL DEFAULT NULL,
	                    PRIMARY KEY (`Id`)
                    )
                    COLLATE='utf8_general_ci'
                    ENGINE=MyIsam
                    AUTO_INCREMENT=0;"
            ).ExecuteUpdate();
        } 
    }
}