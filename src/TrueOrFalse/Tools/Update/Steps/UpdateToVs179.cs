using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs179
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE `widgetview` (
	                  `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                  `Host` VARCHAR(1000) NULL DEFAULT NULL,
	                  `WidgetKey` VARCHAR(1000) NULL DEFAULT NULL,
	                  `DateCreated` DATETIME NULL DEFAULT NULL,
	                  PRIMARY KEY (`Id`),
	                  INDEX `WidgetKey` (`WidgetKey`),
	                  INDEX `Host` (`Host`)
                  )
                  COLLATE='utf8_general_ci'
                  ENGINE=InnoDB
                  AUTO_INCREMENT=6;"
            ).ExecuteUpdate();
        }
    }
}

