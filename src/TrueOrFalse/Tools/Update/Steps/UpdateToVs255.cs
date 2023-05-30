using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs255
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE questionview DROP FOREIGN KEY FK_questionview_game_round;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE game_round;"
            ).ExecuteUpdate();
    }
}