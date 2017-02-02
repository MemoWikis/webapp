using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs165
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `categories_to_questions`
	                ADD INDEX `Category_id` (`Category_id`),
	                ADD INDEX `Question_id` (`Question_id`);"
            ).ExecuteUpdate();
        }
    }
}