using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs057
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"DROP TABLE `knowledgeitem`")
                .ExecuteUpdate();
        }
    }
}