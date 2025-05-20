using System.Collections.Concurrent;

public static class LoggedInSessionStore
{
    private static readonly ConcurrentDictionary<string, byte> _sessions = new();

    public static void Add(string sessionId)
    {
        _sessions.TryAdd(sessionId, 0);
    }

    public static void Remove(string sessionId)
    {
        _sessions.TryRemove(sessionId, out _);
    }

    public static int Count => _sessions.Count;
}
