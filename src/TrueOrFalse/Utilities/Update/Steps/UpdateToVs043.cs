using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs043
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `category`
	                ADD COLUMN `Description` VARCHAR(3000) NULL DEFAULT NULL AFTER `Name`,
	                ADD COLUMN `WikipediaURL` VARCHAR(255) NULL DEFAULT NULL AFTER `Description`;").ExecuteUpdate();
        }
    }
}
