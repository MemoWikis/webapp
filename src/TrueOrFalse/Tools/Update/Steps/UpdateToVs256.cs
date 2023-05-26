using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs256

{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE questionview DROP FOREIGN KEY FK_questionview_game_player;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE game_player;"
            ).ExecuteUpdate();
    }
}