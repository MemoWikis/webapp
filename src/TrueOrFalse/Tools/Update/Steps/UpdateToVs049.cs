using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs049
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `category`
                    ADD COLUMN `TypeJson` TEXT NULL DEFAULT NULL AFTER `Type`;").ExecuteUpdate();
        }
    }
}
