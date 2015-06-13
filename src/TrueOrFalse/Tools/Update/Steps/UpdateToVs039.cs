using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs039
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `question`
	                ADD COLUMN `CorrectnessProbability` INT(11) NULL DEFAULT NULL AFTER `SetsAmount`").ExecuteUpdate();
        }
    }
}
