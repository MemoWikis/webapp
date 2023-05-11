using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs242
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE User CHANGE subscriptionDuration EndDate DATETIME NULL;"
            ).ExecuteUpdate();
    }
}