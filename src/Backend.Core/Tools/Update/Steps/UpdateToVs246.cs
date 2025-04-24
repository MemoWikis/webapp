using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs246
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE membership;"
            ).ExecuteUpdate();
    }
}