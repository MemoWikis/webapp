using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs223
    {
        public static void Run()
        {
            TemplateMigrator.MigrateQuestions();
        }
    }
}