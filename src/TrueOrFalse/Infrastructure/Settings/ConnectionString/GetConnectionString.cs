using System.Configuration;

namespace TrueOrFalse.Infrastructure
{
    public static class GetConnectionString
    {
        public static string Run()
        {
            var result = ReadOverwrittenConfig.ConnectionString();
            if (result.HasValue)
                return result.Value;

            return ConfigurationManager.ConnectionStrings["main"].ConnectionString;
        }
    }
}