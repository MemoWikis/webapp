using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs153
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `reference`
            	    ADD CONSTRAINT `FK_reference_category` FOREIGN KEY (`Category_id`) REFERENCES `category` (`Id`);"
            ).ExecuteUpdate();
        }
    }
}