using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs277
{
    public static void Run(ISession nhibernateSession)
    {
        using (var transaction = nhibernateSession.BeginTransaction())
        {
            try
            {
                // Create 'ai_usage_log' table
                nhibernateSession
                    .CreateSQLQuery(
                        @"CREATE TABLE `ai_usage_log` (
                                `Id` INT NOT NULL AUTO_INCREMENT,
                                `User_id` INT NOT NULL,
                                `Page_id` INT NOT NULL,
                                `TokenIn` INT NOT NULL,
                                `TokenOut` INT NOT NULL,
                                `DateCreated` DATETIME NOT NULL,
                                `Model` VARCHAR(255) NOT NULL,
                                PRIMARY KEY (`Id`)
                            )")
                    .ExecuteUpdate();

                // Create indexes
                nhibernateSession
                    .CreateSQLQuery(
                        @"CREATE INDEX idx_user_id ON ai_usage_log(User_id);
                              CREATE INDEX idx_page_id ON ai_usage_log(Page_id);
                              CREATE INDEX idx_date ON ai_usage_log(DateCreated);
                              CREATE INDEX idx_model ON ai_usage_log(Model);")
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
