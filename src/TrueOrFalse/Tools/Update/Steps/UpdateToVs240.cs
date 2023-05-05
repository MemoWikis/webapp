using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs240
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE user ADD COLUMN subscriptionDuration DATETIME DEFAULT NULL;"
            ).ExecuteUpdate();
    }
}