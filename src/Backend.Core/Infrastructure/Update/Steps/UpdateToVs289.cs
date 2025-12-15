using NHibernate;

internal class UpdateToVs289
{
    public static void Run(ISession nhibernateSession)
    {
        using var transaction = nhibernateSession.BeginTransaction();

        try
        {
            // Create AI model whitelist table
            nhibernateSession.CreateSQLQuery(
                @"CREATE TABLE `aimodelwhitelist` (
                    `Id` INT NOT NULL AUTO_INCREMENT,
                    `ModelId` VARCHAR(100) NOT NULL,
                    `Provider` INT NOT NULL,
                    `TokenCostMultiplier` DECIMAL(10,2) NOT NULL DEFAULT 1.00,
                    `IsEnabled` TINYINT(1) NOT NULL DEFAULT 1,
                    `IsDefault` TINYINT(1) NOT NULL DEFAULT 0,
                    `SortOrder` INT NOT NULL DEFAULT 0,
                    PRIMARY KEY (`Id`),
                    UNIQUE KEY `idx_aimodelwhitelist_modelid` (`ModelId`),
                    INDEX `idx_aimodelwhitelist_provider` (`Provider`),
                    INDEX `idx_aimodelwhitelist_enabled` (`IsEnabled`)
                  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;"
            ).ExecuteUpdate();

            // Insert default suggested models
            // Provider: 0 = Anthropic, 1 = OpenAI
            nhibernateSession.CreateSQLQuery(
                @"INSERT INTO `aimodelwhitelist` (`ModelId`, `Provider`, `TokenCostMultiplier`, `IsEnabled`, `IsDefault`, `SortOrder`) VALUES
                    ('claude-sonnet-4-latest', 0, 1.00, 1, 1, 0),
                    ('claude-3-5-haiku-latest', 0, 0.50, 1, 0, 1),
                    ('claude-3-opus-latest', 0, 5.00, 1, 0, 2),
                    ('gpt-4o', 1, 1.00, 1, 0, 3),
                    ('gpt-4o-mini', 1, 0.30, 1, 0, 4);"
            ).ExecuteUpdate();

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Log.Error(ex, "Failed to execute UpdateToVs289");
            throw;
        }
    }
}
