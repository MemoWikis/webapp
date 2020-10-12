using NHibernate;
using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs213
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `category`
                    ADD COLUMN `Content` TEXT NULL DEFAULT NULL AFTER `TopicMarkdown`"
                ).ExecuteUpdate();
        }
    }
}