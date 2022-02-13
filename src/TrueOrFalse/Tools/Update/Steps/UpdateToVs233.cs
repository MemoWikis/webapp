using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs233
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE categorychange ADD COLUMN `Parent_Category_Id` Int DEFAULT NULL;"
                ).ExecuteUpdate();
        }
    }
}