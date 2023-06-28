using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace TrueOrFalse.Infrastructure.Persistence
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
                    String.Format(@"SELECT COUNT(table_name)
                                    FROM information_schema.tables
                                    WHERE table_schema = '{0}'
                                    AND table_name = '{1}'", 
                                    _session.Connection.Database, 
                                    tableName))
                    .UniqueResult<object>();

            return Convert.ToInt32(result) == 1;
        }
    }
}
