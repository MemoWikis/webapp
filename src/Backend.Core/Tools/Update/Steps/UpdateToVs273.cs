using NHibernate;

internal class UpdateToVs273
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE questionview ADD COLUMN DateOnly DATE GENERATED ALWAYS AS (DATE(DateCreated)) VIRTUAL;"
            ).ExecuteUpdate();

        //nhibernateSession
        //    .CreateSQLQuery(
        //        @"CREATE INDEX idx_dateonly ON questionview(DateOnly);"
        //    ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE categoryview ADD COLUMN DateOnly DATE GENERATED ALWAYS AS (DATE(DateCreated)) VIRTUAL;"
            ).ExecuteUpdate();

        //nhibernateSession
        //    .CreateSQLQuery(
        //        @"CREATE INDEX idx_dateonly ON categoryview(DateOnly);"
        //    ).ExecuteUpdate();
    }
}