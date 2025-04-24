using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs272
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE user ADD COLUMN LastLogin DATETIME;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"UPDATE user SET LastLogin = DateCreated;"
            ).ExecuteUpdate();
    }
}