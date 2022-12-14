using NHibernate;

namespace TrueOrFalse.Updates
{
    class UpdateToVs238
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `user`
	                CHANGE COLUMN `AddToWikiHistory` `RecentlyUsedRelationTargetTopics` TEXT NULL DEFAULT NULL COLLATE 'utf8mb3_general_ci' AFTER `MailBounceReason`;"
                ).ExecuteUpdate();
            CategoryAuthorUpdater.UpdateAll();
        }
    }
}