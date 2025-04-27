using NHibernate;

internal class UpdateToVs267
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE relatedcategoriestorelatedcategories ADD Previous_id INT NULL;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE relatedcategoriestorelatedcategories ADD Next_id INT NULL;"
            ).ExecuteUpdate();
    }
}