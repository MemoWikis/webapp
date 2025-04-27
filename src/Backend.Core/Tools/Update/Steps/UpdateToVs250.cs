using NHibernate;

internal class UpdateToVs250
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE badge;"
            ).ExecuteUpdate();
    }
}