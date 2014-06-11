using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs054
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `comment`
	                 CHANGE COLUMN `ShouldIds` `ShouldKeys` VARCHAR(255) NULL DEFAULT NULL AFTER `ShouldRemove`;")
                .ExecuteUpdate();
        }
    }
}
