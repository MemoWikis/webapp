using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs045
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `imagemetadata`
	                ADD COLUMN `Author` TEXT NULL AFTER `ApiResult`,
	                ADD COLUMN `Description` TEXT NULL AFTER `Author`,
	                ADD COLUMN `Licence` TEXT NULL AFTER `Description`;").ExecuteUpdate();
        }
    }
}
