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
                    @"ALTER TABLE `question`
                         DROP COLUMN `License`,
                         ADD COLUMN `License` INT(11) NULL DEFAULT NULL AFTER `Description`;
             
                      UPDATE `question` SET `License` = 1;"

            ).ExecuteUpdate();
        }
    }
}