using NHibernate;

internal class UpdateToVs251
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE categories_to_sets DROP FOREIGN KEY FK937EA8C2253EAD1B;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE categories_to_sets DROP FOREIGN KEY FK937EA8C286903877;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE categories_to_sets;"
            ).ExecuteUpdate();
    }
}