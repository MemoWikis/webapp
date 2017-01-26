using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs162
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `user`
	            ADD COLUMN `KnowledgeReportInterval` INT NOT NULL DEFAULT '0' AFTER `ShowWishKnowledge`;"
            ).ExecuteUpdate();
        }
    }
}