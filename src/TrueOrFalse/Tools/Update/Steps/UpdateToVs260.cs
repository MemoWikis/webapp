using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs260
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE questioninset DROP FOREIGN KEY FKD34040F20054BCB;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE questioninset DROP FOREIGN KEY FKD34040F3A925A27;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE questioninset;"
            ).ExecuteUpdate();
    }
}