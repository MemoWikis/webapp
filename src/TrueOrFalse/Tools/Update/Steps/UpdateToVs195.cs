using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs195
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `imagemetadata`
	                ADD INDEX `Type-Index` (`Type`),
	                ADD INDEX `TypeId-Index` (`TypeId`);"
            ).ExecuteUpdate();
        }
    }
}