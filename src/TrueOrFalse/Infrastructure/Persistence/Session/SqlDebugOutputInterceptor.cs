using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NHibernate;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using Serilog;

public class SqlDebugOutputInterceptor : EmptyInterceptor
{
    private ISession _session;
    private readonly Dictionary<string, Stopwatch> _watches = new();

    public override void SetSession(ISession session)
    {
        base.SetSession(session);
        _session = session;
        Log.Information("NHibernate SetSession");

        _watches.Add(((SessionImpl)_session).SessionId.ToString(), Stopwatch.StartNew());
    }

    public override SqlString OnPrepareStatement(SqlString sql)
    {
        var sqlString = sql.ToString();
        StackTrace stackTrace = new StackTrace(true);
        var statistics = ((SessionImpl)_session).Statistics;
        var sessionId = ((SessionImpl)_session).SessionId.ToString();
        Task.Run(() =>
        {
            var stackFrames = stackTrace.GetFrames();
            var sb = new StringBuilder();
            foreach (var frame in stackFrames)
            {
                var method = frame.GetMethod();
                if (!method.Module.Name.Contains("TrueOrFalse"))
                    continue;

                sb.AppendLine($"{method.DeclaringType}.{method.Name} in {frame.GetFileName()}:{frame.GetFileLineNumber()}");
            }

            if (_session.IsOpen)
                Log.Information("NHibernate before query: {sessionId} collections {collectionCount} entities {entityCount}",
                    sessionId, statistics.CollectionCount, statistics.EntityCount);

            if(_watches.ContainsKey(sessionId))
                Log.Information("NHibernate before elapsed: {elapsed}", _watches[sessionId].Elapsed.TotalMilliseconds);
            
            Log.Information("NHibernate {sessionId} {sqlString} {stacktrace}", ((SessionImpl)_session).SessionId, sqlString, sb);
        });

        return base.OnPrepareStatement(sql);
    }

    public override void PostFlush(ICollection entities)
    {
        var statistics = ((SessionImpl) _session).Statistics;

        var sessionId = ((SessionImpl)_session).SessionId.ToString();
        if(_watches.ContainsKey(sessionId))
            _watches.Remove(sessionId);

        if (!_session.IsOpen)
            return;

        Log.Information("NHibernate post flush {sessionId} collections {collectionCount} entities {entityCount}",
            sessionId, statistics.CollectionCount, statistics.EntityCount);

        base.PostFlush(entities);
    }
}