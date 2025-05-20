using System.Collections.Concurrent;

public static class LoggedInSessionStore
{
    private static readonly ConcurrentDictionary<string, DateTime> _sessions = new();

    public static void Add(string sessionId)
    {
        _sessions[sessionId] = DateTime.UtcNow;
    }

    public static void Touch(string sessionId)
    {
        if (_sessions.ContainsKey(sessionId))
            _sessions[sessionId] = DateTime.UtcNow;
    }

    public static void Remove(string sessionId)
    {
        _sessions.TryRemove(sessionId, out _);
    }

    public static IEnumerable<string> GetSessionsActiveWithin(TimeSpan timeSpan)
    {
        var threshold = DateTime.UtcNow - timeSpan;
        return _sessions
            .Where(s => s.Value >= threshold)
            .Select(s => s.Key)
            .ToList();
    }

    public static int CountActiveWithin(TimeSpan timeSpan)
    {
        var threshold = DateTime.UtcNow - timeSpan;
        return _sessions.Count(s => s.Value >= threshold);
    }

    public static int Count => _sessions.Count;
}
