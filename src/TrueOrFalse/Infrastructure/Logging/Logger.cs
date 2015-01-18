using Serilog;

public class Logg
{
    public static ILogger r()
    {
        return new LoggerConfiguration()
            .WriteTo.Seq("http://localhost:5341")
            .CreateLogger();
    }
}
