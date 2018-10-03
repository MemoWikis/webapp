using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs194
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `categorychange` 
                    ADD COLUMN `Type` INT NULL AFTER `Data`;"
            ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(@"UPDATE `categorychange` SET Type = 1;")
                .ExecuteUpdate();
        }

    }
}

