using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs243
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE User CHANGE subscriptionStartDate SubscriptionStartDate DATETIME NULL;"
            ).ExecuteUpdate();
    }
}