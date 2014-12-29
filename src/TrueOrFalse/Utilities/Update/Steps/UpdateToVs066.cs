using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs066
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `question`
	                ADD COLUMN `IsWorkInProgress` BIT NULL DEFAULT b'0' AFTER `SetsTop5Json`;")
                 .ExecuteUpdate();
        } 
    }
}
