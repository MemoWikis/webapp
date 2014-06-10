using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs052
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `comment`
	                ADD COLUMN `ShouldRemove` BIT NULL DEFAULT NULL AFTER `Text`,
	                ADD COLUMN `ShouldImprove` BIT NULL DEFAULT NULL AFTER `ShouldRemove`,
	                ADD COLUMN `ShouldIds` VARCHAR(50) NULL DEFAULT NULL AFTER `ShouldImprove`;")
                .ExecuteUpdate();
        }
    }
}
