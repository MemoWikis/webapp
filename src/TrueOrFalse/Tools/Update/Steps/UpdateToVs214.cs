using NHibernate;
using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs214
    {
        public static void Run()
        {
            TemplateMigrator.MigrateTopicMarkdownToContent();
        }
    }
}