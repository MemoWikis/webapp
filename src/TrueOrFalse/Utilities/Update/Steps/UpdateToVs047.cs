using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs047
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `imagemetadata`
	                ADD COLUMN `ApiHost` VARCHAR(255) NULL DEFAULT NULL AFTER `ApiResult`;").ExecuteUpdate();

            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"UPDATE imagemetadata SET ApiHost = 'commons.wikimedia.org'").ExecuteUpdate();
        }
    }
}
