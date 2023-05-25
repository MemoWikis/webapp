using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs251
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE categories_to_sets;"
            ).ExecuteUpdate();
    }
}