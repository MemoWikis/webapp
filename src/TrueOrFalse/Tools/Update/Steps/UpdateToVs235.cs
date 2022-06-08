using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs235
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"DELETE FROM relatedcategoriestorelatedcategories WHERE CategoryRelationType = 2;
                    ALTER TABLE relatedcategoriestorelatedcategories DROP CategoryRelationType;"
                ).ExecuteUpdate();
            CategoryAuthorUpdater.UpdateAll();
        }
    }
}