using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs243
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE User CHANGE subscriptionStartDate SubscriptionStartDate DATETIME NULL;"
            ).ExecuteUpdate();
    }
}