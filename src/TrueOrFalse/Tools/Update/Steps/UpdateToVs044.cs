using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs044
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `imagemetadata` CHANGE COLUMN `LicenceInfo` `ApiResult` TEXT NULL AFTER `SourceUrl`;").ExecuteUpdate();
        }
    }
}
