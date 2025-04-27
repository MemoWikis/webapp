using NHibernate;

internal class UpdateToVs241
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE user ADD COLUMN subscriptionStartDate DATETIME DEFAULT NULL;"
            ).ExecuteUpdate();
    }
}