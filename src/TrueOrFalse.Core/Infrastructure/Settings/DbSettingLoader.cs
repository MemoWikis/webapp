using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core.Infrastructure
{
    public class DbSettingsStorage : IRegisterAsInstancePerLifetime
    {
        public DbSettings Get()
        {
            return new DbSettings();
        }

        public void Update(DbSettings dbSettings)
        {
            
        }
    }
}
