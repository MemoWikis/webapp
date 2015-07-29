using Serilog;

public class Logg
{
    private static ILogger _logger;

    public static ILogger r()
    {
        if (_logger == null) {     
            _logger = new LoggerConfiguration()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();
        }

        return _logger;
    }
}