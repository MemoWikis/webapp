using NHibernate;

internal class UpdateToVs254
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE game_to_sets;"
            ).ExecuteUpdate();
    }
}