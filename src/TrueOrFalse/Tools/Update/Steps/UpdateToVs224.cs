using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs224
    {
        public static void Run()
        {
            TopicNavigationMigration.Run();
        }
    }
}