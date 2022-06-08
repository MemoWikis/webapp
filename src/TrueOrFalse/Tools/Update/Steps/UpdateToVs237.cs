using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs237
    {
        public static void Run()
        {
            WuwiMigrator.CreateWuwiCategoryForAllUsers();
        }
    }
}