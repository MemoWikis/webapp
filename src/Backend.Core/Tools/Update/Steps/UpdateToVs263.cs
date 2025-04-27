using NHibernate;

internal class UpdateToVs263
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE setview;"
            ).ExecuteUpdate();
    }
}