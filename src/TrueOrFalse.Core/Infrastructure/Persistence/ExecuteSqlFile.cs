using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NHibernate;

namespace TrueOrFalse.Core.Infrastructure.Persistence
{
    public class ExecuteSqlFile : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public ExecuteSqlFile(ISession session){
            _session = session;
        }

        public void Run(string filePath)
        {   
            _session.CreateSQLQuery(File.ReadAllText(filePath)).ExecuteUpdate();    
        }
    }
}
