using NHibernate;

internal class UpdateToVs252
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE date_to_sets DROP FOREIGN KEY FKCB583B0E93032C55;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE date_to_sets DROP FOREIGN KEY FKCB583B0EBC787FC9;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE date_to_sets;"
            ).ExecuteUpdate();
    }
}