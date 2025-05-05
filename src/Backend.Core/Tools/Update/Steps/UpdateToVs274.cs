using NHibernate;

internal class UpdateToVs274
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE user DROP COLUMN LastLogin;"
            ).ExecuteUpdate();
    }
}