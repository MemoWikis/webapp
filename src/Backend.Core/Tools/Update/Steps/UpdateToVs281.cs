using NHibernate;

internal class UpdateToVs281
{
    public static void Run(ISession nhibernateSession)
    {
        using var transaction = nhibernateSession.BeginTransaction();

        try
        {
            nhibernateSession.CreateSQLQuery(
                    @"ALTER TABLE `page`
                  ADD COLUMN `Language` VARCHAR(5) NOT NULL DEFAULT 'de';")
                .ExecuteUpdate();

            nhibernateSession.CreateSQLQuery(
                    @"ALTER TABLE `user`
                  ADD COLUMN `UiLanguage` VARCHAR(5) NOT NULL DEFAULT 'de';")
                .ExecuteUpdate();


            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.Error.WriteLine($"An error occurred during the update: {ex.Message}");
            Logg.r.Error($"An error occurred during the update: {ex.Message}", ex);
            throw;
        }
    }
}
