using NHibernate;

namespace TrueOrFalse.Updates
{
    internal class UpdateToVs276
    {
        public static void Run(ISession nhibernateSession)
        {
            using (var transaction = nhibernateSession.BeginTransaction())
            {
                try
                {
                    // Rename Tables
                    nhibernateSession
                        .CreateSQLQuery(
                            @"RENAME TABLE 
                                `categories_to_questions` TO `pages_to_questions`,
                                `category` TO `page`,
                                `categorychange` TO `pagechange`,
                                `categoryvaluation` TO `pagevaluation`,
                                `categoryview` TO `pageview`,
                                `relatedcategoriestorelatedcategories` TO `pagerelation`")
                        .ExecuteUpdate();

                    // Rename Columns in 'pages_to_questions'
                    nhibernateSession
                        .CreateSQLQuery(
                            @"ALTER TABLE `pages_to_questions` 
                                RENAME COLUMN `Category_id` TO `Page_id`")
                        .ExecuteUpdate();

                    // Rename Columns in 'page'
                    nhibernateSession
                        .CreateSQLQuery(
                            @"ALTER TABLE `page` 
                                RENAME COLUMN `IsUserStartTopic` TO `IsWiki`")
                        .ExecuteUpdate();

                    // Rename Columns in 'pagechange'
                    nhibernateSession
                        .CreateSQLQuery(
                            @"ALTER TABLE `pagechange` 
                                RENAME COLUMN `Category_id` TO `Page_id`,
                                RENAME COLUMN `Parent_Category_Ids` TO `Parent_Page_Ids`")
                        .ExecuteUpdate();

                    // Rename Columns in 'pagevaluation'
                    nhibernateSession
                        .CreateSQLQuery(
                            @"ALTER TABLE `pagevaluation` 
                                RENAME COLUMN `CategoryId` TO `PageId`")
                        .ExecuteUpdate();

                    // Rename Columns in 'pageview'
                    nhibernateSession
                        .CreateSQLQuery(
                            @"ALTER TABLE `pageview` 
                                RENAME COLUMN `Category_id` TO `Page_id`")
                        .ExecuteUpdate();

                    // Rename Columns in 'learningsession'
                    nhibernateSession
                        .CreateSQLQuery(
                            @"ALTER TABLE `learningsession` 
                                RENAME COLUMN `CategoryToLearn_id` TO `PageToLearn_id`")
                        .ExecuteUpdate();

                    // Rename Columns in 'reference'
                    nhibernateSession
                        .CreateSQLQuery(
                            @"ALTER TABLE `reference` 
                                RENAME COLUMN `Category_id` TO `Page_id`")
                        .ExecuteUpdate();

                    // Rename Columns in 'pagerelation'
                    nhibernateSession
                        .CreateSQLQuery(
                            @"ALTER TABLE `pagerelation` 
                                RENAME COLUMN `Category_id` TO `Page_id`")
                        .ExecuteUpdate();

                    // Rename Columns in 'user'
                    nhibernateSession
                        .CreateSQLQuery(
                            @"ALTER TABLE `user` 
                                RENAME COLUMN `StartTopicId` TO `StartPageId`")
                        .ExecuteUpdate();

                    // Add New Columns to 'user' Table
                    nhibernateSession
                        .CreateSQLQuery(
                            @"ALTER TABLE `user` 
                                ADD COLUMN `Wikis` JSON NULL,
                                ADD COLUMN `Favorites` JSON NULL")
                        .ExecuteUpdate();

                    // Rename Columns in 'useractivity'
                    nhibernateSession
                        .CreateSQLQuery(
                            @"ALTER TABLE `useractivity` 
                                RENAME COLUMN `Category_id` TO `Page_id`")
                        .ExecuteUpdate();


                    // Commit the transaction if all operations succeed
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    Console.Error.WriteLine($"An error occurred during the update: {ex.Message}");
                    Console.Error.WriteLine(ex.StackTrace);
                    Logg.r.Error($"An error occurred during the update: {ex.Message}", ex);

                    throw;
                }
            }
        }
    }
}
