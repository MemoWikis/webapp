using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Seedworks.Web.State;

namespace TrueOrFalse.Core.Infrastructure
{
    public static class GetConnectionString
    {
        public static string Run()
        {
            var result = ReadOverwrittenConnectionString.Run();
            if (result.HasValue)
                return result.Value;

            return ConfigurationManager.ConnectionStrings["main"].ConnectionString;
        }
    }
}
