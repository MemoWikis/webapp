using System.Configuration;

namespace TrueOrFalse.Infrastructure
{
    public static class GetConnectionString
    {
        public static string Run()
        {
            var result = OverwrittenConfig.ConnectionString();
            if (result.HasValue)
                return result.Value;

            return ConfigurationManager.ConnectionStrings["main"].ConnectionString;
        }
    }
}