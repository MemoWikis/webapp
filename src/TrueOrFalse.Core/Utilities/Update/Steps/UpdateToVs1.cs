using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NHibernate;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs1
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ExecuteSqlFile>().Run("Utilities/Update/Scripts/1-create-setting-tbl.sql");
            Console.WriteLine("update to 1");
        }
    }
}
