using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs270
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE category ADD COLUMN IsHideText TINYINT(1) DEFAULT 0;"
            ).ExecuteUpdate();
    }
}