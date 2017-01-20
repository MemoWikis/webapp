using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs111
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(@"ALTER TABLE `user_to_follower`
	                ADD COLUMN `Id` INT(11) NOT NULL AUTO_INCREMENT FIRST,
	                ADD COLUMN `DateCreated` DATETIME NULL DEFAULT NULL AFTER `Follower_id`,
	                ADD COLUMN `DateModified` DATETIME NULL DEFAULT NULL AFTER `DateCreated`,
	                ADD PRIMARY KEY (`Id`);"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>().CreateSQLQuery(@"update user_to_follower set DateCreated = now()").ExecuteUpdate();
            Sl.Resolve<ISession>().CreateSQLQuery(@"update user_to_follower set DateModified = now()").ExecuteUpdate();


        }
    }
}