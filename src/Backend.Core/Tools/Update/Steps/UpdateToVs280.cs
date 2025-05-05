using NHibernate;

internal class UpdateToVs280
{
    public static void Run(ISession nhibernateSession)
    {
        using var transaction = nhibernateSession.BeginTransaction();

        try
        {
            nhibernateSession.CreateSQLQuery(
                @"ALTER TABLE `question` DROP COLUMN `IsWorkInProgress`;"
            ).ExecuteUpdate();

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.Error.WriteLine($"An error occurred during the update: {ex.Message}");
            Console.Error.WriteLine(ex.StackTrace);
            Log.Error($"An error occurred during the update: {ex.Message}", ex);

            throw;
        }
    }
}