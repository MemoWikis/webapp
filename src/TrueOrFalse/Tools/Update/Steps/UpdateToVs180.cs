using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs180
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `widgetview`
	                ADD COLUMN `EntityId` INT(11) NOT NULL AFTER `WidgetType`;"
            ).ExecuteUpdate();
        }
    }
}

