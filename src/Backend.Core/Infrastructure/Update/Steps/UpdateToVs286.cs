using NHibernate;

internal class UpdateToVs286
{
    public static void Run(ISession nhibernateSession)
    {
        using var transaction = nhibernateSession.BeginTransaction();

        try
        {
            nhibernateSession.CreateSQLQuery(
                @"ALTER TABLE `page` DROP COLUMN `TypeJson`;"
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
