using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs262
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE setvaluation;"
            ).ExecuteUpdate();
    }
}