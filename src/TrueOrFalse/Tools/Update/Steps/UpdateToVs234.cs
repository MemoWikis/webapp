using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs234
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE categorychange RENAME COLUMN `Parent_Category_Id` To `Parent_Category_Ids`;
                    ALTER TABLE categorychange Modify `Parent_Category_Ids` Text;"
                ).ExecuteUpdate();
        }
    }
}