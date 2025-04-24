using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs240
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE user ADD COLUMN subscriptionDuration DATETIME DEFAULT NULL;"
            ).ExecuteUpdate();
    }
}