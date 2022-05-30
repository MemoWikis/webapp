using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs235
    {
        public static void Run()
        {
            WuwiMigrator.CreateWuwiCategoryForAllUsers();
        }
    }
}