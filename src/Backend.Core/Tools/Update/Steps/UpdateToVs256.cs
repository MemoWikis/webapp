using NHibernate;

internal class UpdateToVs256

{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE questionview DROP FOREIGN KEY FK_questionview_game_player;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE game_player;"
            ).ExecuteUpdate();
    }
}