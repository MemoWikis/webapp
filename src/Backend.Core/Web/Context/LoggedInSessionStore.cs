using System.Collections.Concurrent;

public static class LoggedInSessionStore
{
    private static readonly ConcurrentDictionary<string, int> _sessions = new();

    public static void Add(string sessionId, int userId)
    {
        if (string.IsNullOrEmpty(sessionId))
            return;
        _sessions[sessionId] = userId;
    }

    public static void Remove(string sessionId)
    {
        if (string.IsNullOrEmpty(sessionId))
            return;
        _sessions.TryRemove(sessionId, out _);
    }

    public static int Count => _sessions.Count;
}
