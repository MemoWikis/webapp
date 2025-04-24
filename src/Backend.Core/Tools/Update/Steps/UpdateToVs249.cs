using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs249
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE appaccess;"
            ).ExecuteUpdate();
    }
}