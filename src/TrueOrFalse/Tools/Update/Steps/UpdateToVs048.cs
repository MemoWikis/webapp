using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs048
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `category`
	                ADD COLUMN `Type` Int NULL DEFAULT NULL AFTER `CountCreators`;").ExecuteUpdate();

            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"UPDATE category SET Type = 1").ExecuteUpdate();
        }
    }
}
