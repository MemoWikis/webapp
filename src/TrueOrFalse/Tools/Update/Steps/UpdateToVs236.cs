using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs236
    {
        public static void Run()
        {
            WuwiMigrator.CreateWuwiCategoryForAllUsers();
        }
    }
}