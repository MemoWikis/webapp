using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs241
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE user ADD COLUMN subscriptionStartDate DATETIME DEFAULT NULL;"
            ).ExecuteUpdate();
    }
}