using System.Text;

namespace Backend.Core.Infrastructure.Persistence.Session;

public static class ConnectionStringHelper
{
    /// <summary>
    /// Ensures the connection string has proper timeout settings for bulk operations
    /// </summary>
    /// <param name="connectionString">The base connection string</param>
    /// <returns>Connection string with timeout settings</returns>
    public static string EnsureTimeoutSettings(string connectionString)
    {
        var builder = new StringBuilder(connectionString);
        
        // Add or update connection timeout (time to wait for connection to establish)
        if (!connectionString.Contains("Connection Timeout", StringComparison.OrdinalIgnoreCase) &&
            !connectionString.Contains("ConnectionTimeout", StringComparison.OrdinalIgnoreCase))
        {
            if (!connectionString.EndsWith(";"))
                builder.Append(";");
            builder.Append("Connection Timeout=300;"); // 5 minutes
        }
        
        // Add or update command timeout (time to wait for command execution)
        if (!connectionString.Contains("Default Command Timeout", StringComparison.OrdinalIgnoreCase))
        {
            if (!connectionString.EndsWith(";"))
                builder.Append(";");
            builder.Append("Default Command Timeout=300;"); // 5 minutes
        }
        
        // Ensure we can handle large result sets
        if (!connectionString.Contains("AllowBatch", StringComparison.OrdinalIgnoreCase))
        {
            if (!connectionString.EndsWith(";"))
                builder.Append(";");
            builder.Append("AllowBatch=true;");
        }
        
        return builder.ToString();
    }
}