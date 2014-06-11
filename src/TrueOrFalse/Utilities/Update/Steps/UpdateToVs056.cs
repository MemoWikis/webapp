using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs056
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `message`
	                CHANGE COLUMN `Subject` `Subject` VARCHAR(500) NULL DEFAULT NULL AFTER `ReceiverId`,
	                CHANGE COLUMN `Body` `Body` TEXT NULL DEFAULT NULL AFTER `Subject`;")
                .ExecuteUpdate();
        }
    }
}