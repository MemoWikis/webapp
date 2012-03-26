using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace TrueOrFalse.Core.Infrastructure.Persistence
{
    public class DoesTableExist : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public DoesTableExist(ISession session){ _session = session; }

        public bool Run(string tableName)
        {
            tableName = tableName.Replace("[", "").Replace("]", "");

            var result =
                _session.CreateSQLQuery(
                    String.Format("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'", tableName))
                    .UniqueResult<int>();

            return result == 1;
        }
    }
}
