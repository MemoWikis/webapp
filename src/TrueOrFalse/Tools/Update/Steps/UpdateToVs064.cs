using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs064
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `reference`
	                DROP COLUMN `Index`;")
                .ExecuteUpdate();
        }
    }
}
