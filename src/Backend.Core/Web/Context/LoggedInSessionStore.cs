using System.Collections.Concurrent;
using System.Timers;

public static class LoggedInSessionStore
{
    private static readonly ConcurrentDictionary<string, DateTime> _loggedInSessions = new();
    private static readonly ConcurrentDictionary<string, DateTime> _anonymousSessions = new();
    private static readonly System.Timers.Timer _cleanupTimer;
    private const int CleanupIntervalMinutes = 5;

    static LoggedInSessionStore()
    {
        _cleanupTimer = new System.Timers.Timer(CleanupIntervalMinutes * 60 * 1000)
        {
            AutoReset = false
        };
        _cleanupTimer.Elapsed += (s, e) =>
        {
            try { CleanupOldSessions(); }
            finally
            {
                _cleanupTimer.Start();
            }
        };
        _cleanupTimer.Start();
    }

    private static void CleanupOldSessions()
    {
        var cutoffTime = DateTime.UtcNow.AddMinutes(-CleanupIntervalMinutes);

        foreach (var session in _loggedInSessions)
            if (session.Value < cutoffTime) 
                _loggedInSessions.TryRemove(session.Key, out _);

        foreach (var session in _anonymousSessions)
            if (session.Value < cutoffTime)
                _anonymousSessions.TryRemove(session.Key, out _);
    }

    public static void TouchLoggedInUsers(string sessionId) => 
        _loggedInSessions.AddOrUpdate(sessionId, DateTime.UtcNow, (_, _) => DateTime.UtcNow);

    public static void TouchOrAddAnonymousUsers(string sessionId) => 
        _anonymousSessions.AddOrUpdate(sessionId, DateTime.UtcNow, (_, _) => DateTime.UtcNow);

    public static void Remove(string sessionId) => 
        _loggedInSessions.TryRemove(sessionId, out _);

    public static int GetLoggedInUsersActiveWithin(TimeSpan timeSpan)
    {
        var threshold = DateTime.UtcNow - timeSpan;
        return _loggedInSessions
            .Count(s => s.Value >= threshold);
    }

    public static int GetAnonymousActiveWithin(TimeSpan timeSpan)
    {
        var threshold = DateTime.UtcNow - timeSpan;
        return _anonymousSessions
            .Count(s => s.Value >= threshold);
    }
}
