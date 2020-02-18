
using ISession = NHibernate.ISession;


namespace TrueOrFalse.Updates
{
    public class UpdateToVs210
    {
        public static void Run()
        {
            var session = Sl.Resolve<ISession>();

            session
                .CreateSQLQuery(
                    @"ALTER TABLE `category` ADD `FormerSetId` INT;"
                ).ExecuteUpdate();

        }
    }
}