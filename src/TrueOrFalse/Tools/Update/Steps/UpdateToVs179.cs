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
                    `Host` VARCHAR(255) NULL DEFAULT NULL,
                    `WidgetKey` VARCHAR(255) NULL DEFAULT NULL,
                    `WidgetType` TINYINT NOT NULL,
                    `DateCreated` DATETIME NULL DEFAULT NULL,
                    PRIMARY KEY (`Id`),
                    INDEX `WidgetKey` (`WidgetKey`),
                    INDEX `Host` (`Host`)
                  )
                  COLLATE='utf8_general_ci'
                  ENGINE=InnoDB;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `questionview`
                      ADD COLUMN `WidgetView_id` INT(11) NULL DEFAULT NULL AFTER `Player_id`,
                      ADD INDEX `FK_WidgetView` (`WidgetView_id`),
                      ADD CONSTRAINT `FK_WidgetView` FOREIGN KEY(`WidgetView_id`) REFERENCES `widgetview` (`Id`);"
                ).ExecuteUpdate();
        }
    }
}

