using NHibernate;

internal class UpdateToVs279
{
    public static void Run(ISession nhibernateSession)
    {
        using var transaction = nhibernateSession.BeginTransaction();

        try
        {
            nhibernateSession.CreateSQLQuery(
                @"ALTER TABLE `jobqueue`
                          ADD COLUMN `DateCreated` DATETIME NULL;"
            ).ExecuteUpdate();

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