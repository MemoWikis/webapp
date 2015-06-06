using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs070
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>().CreateSQLQuery(
                @"ALTER TABLE `user`
	                ADD COLUMN `ShowWishKnowledge` BIT NULL DEFAULT b'0' AFTER `AllowsSupportiveLogin`;")
                 .ExecuteUpdate();
        } 
    }
}
