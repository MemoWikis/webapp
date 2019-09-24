using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs204
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
            @"ALTER TABLE `memucho`.`questionview` 
            CHANGE COLUMN `UserId` `UserId` INT(10) NULL ;")
                .ExecuteUpdate();

            
        }
    }
}