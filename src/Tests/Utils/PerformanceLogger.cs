
using System.Diagnostics;

public sealed class PerformanceLogger
{
    private readonly bool _enabled;
    private readonly string _prefix;
    private readonly Stopwatch _stopwatch = new();

    public PerformanceLogger(string prefix = "PERF", bool enabled = false)
    {
        _enabled = enabled;
        _prefix = prefix;

        if (enabled)
            _stopwatch.Start();
    }

    /// <summary>
    /// Writes a performance line and restarts the stopwatch.
    /// </summary>
    public void Log(string message)
    {
        if (!_enabled)
            return;

        Console.WriteLine(
            $"[{_prefix} {DateTime.Now:HH:mm:ss.fff}] " +
            $"{message} ({_stopwatch.ElapsedMilliseconds:N0} ms)");

        _stopwatch.Restart();
    }
}