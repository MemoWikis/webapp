using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs267
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE questionview DROP FOREIGN KEY FK_WidgetView;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE widgetview;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE useractivity DROP COLUMN Date_id DROP COLUMN Gamer_id DROP COLUMN Set_id;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE questionview DROP COLUMN Round_id DROP COLUMN WidgetView_id DROP COLUMN Planer_id;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE message DROP COLUMN TrainingDate_id;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE learningsession DROP COLUMN DateToLearn_id DROP COLUMN SetToLearn_id;"
            ).ExecuteUpdate();
    }
}