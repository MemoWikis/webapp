using System;
using System.Linq;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs145
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE `jobqueue` (
	                `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                `JobQueueType` INT(11) NULL DEFAULT NULL,
	                `JobContent` VARCHAR(1000) NULL DEFAULT NULL,
	                PRIMARY KEY (`Id`)
                )
                COLLATE='utf8_general_ci'
                ENGINE=MyIsam;"
            ).ExecuteUpdate();

        }
    }
}