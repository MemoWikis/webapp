using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs254
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE game_to_sets;"
            ).ExecuteUpdate();
    }
}