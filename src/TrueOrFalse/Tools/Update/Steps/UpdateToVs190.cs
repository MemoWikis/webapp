using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs190
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `categorychange`
	                DROP FOREIGN KEY `FK_Category_id`;"
            ).ExecuteUpdate();
        }
    }
}

