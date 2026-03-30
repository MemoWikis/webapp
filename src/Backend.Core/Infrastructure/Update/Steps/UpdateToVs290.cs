using NHibernate;

internal class UpdateToVs290
{
    public static void Run(ISession nhibernateSession)
    {
        using var transaction = nhibernateSession.BeginTransaction();

        try
        {
            // Add pricing columns to aimodelwhitelist
            nhibernateSession.CreateSQLQuery(
                @"ALTER TABLE `aimodelwhitelist`
                  ADD COLUMN `InputPricePerMillion` DECIMAL(10,4) NOT NULL DEFAULT 0.0000,
                  ADD COLUMN `OutputPricePerMillion` DECIMAL(10,4) NOT NULL DEFAULT 0.0000;"
            ).ExecuteUpdate();

            // Remove obsolete columns from aimodelwhitelist
            nhibernateSession.CreateSQLQuery(
                @"ALTER TABLE `aimodelwhitelist`
                  DROP COLUMN `IsDefault`,
                  DROP COLUMN `SortOrder`;"
            ).ExecuteUpdate();

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Log.Error(ex, "Failed to execute UpdateToVs290");
            throw;
        }
    }
}
