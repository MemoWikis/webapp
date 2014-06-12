using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs055
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `comment` CHANGE COLUMN `Text` `Text` TEXT NULL DEFAULT NULL AFTER `ShouldKeys`;")
                .ExecuteUpdate();
        }
    }
}
