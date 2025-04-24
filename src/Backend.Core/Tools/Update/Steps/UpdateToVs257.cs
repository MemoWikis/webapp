using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs257

{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE useractivity DROP FOREIGN KEY Game_id_FK;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE game;"
            ).ExecuteUpdate();
    }
}