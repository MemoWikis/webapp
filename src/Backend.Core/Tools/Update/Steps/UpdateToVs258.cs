using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs258
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE questionfeature;"
            ).ExecuteUpdate();
    }
}