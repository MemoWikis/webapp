using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using Serilog;

public class SqlDebugOutputInterceptor : EmptyInterceptor
{
    private ISession _session;

    public override void SetSession(ISession session)
    {
        base.SetSession(session);
        _session = session;
    }

    public override SqlString OnPrepareStatement(SqlString sql)
    {
        var sqlString = sql.ToString();
        StackTrace stackTrace = new StackTrace(true);
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

            Log.Information("NHibernate {sessionId} {sqlString} {stacktrace}", ((SessionImpl)_session).SessionId, sqlString, sb);
        });

        return base.OnPrepareStatement(sql);
    }

    public override void PostFlush(ICollection entities)
    {
        var statistics = ((SessionImpl) _session).Statistics;

        Log.Information("NHibernate post flush {sessionId} collections {collectionCount} entities {entityCound}",
            statistics.CollectionCount, statistics.EntityCount);

        base.PostFlush(entities);
    }
}