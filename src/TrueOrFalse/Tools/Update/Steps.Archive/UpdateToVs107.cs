using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs107
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(@"RENAME TABLE `categoriestoquestions` TO `categories_to_questions`;"
            ).ExecuteUpdate();
        }
    }
}