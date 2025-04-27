using NHibernate;

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