using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs272
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE user ADD COLUMN LastLogin DATETIME NOT NULL DEFAULT '2011-01-26 14:30:00';"
            ).ExecuteUpdate();
    }
}