using System.Configuration;

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