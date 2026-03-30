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
                @"CREATE TABLE IF NOT EXISTS `aimodelwhitelist` (
                    `Id` INT NOT NULL AUTO_INCREMENT,
                    `ModelId` VARCHAR(100) NOT NULL,
                    `DisplayName` VARCHAR(200) NULL,
                    `Provider` INT NOT NULL,
                    `TokenCostMultiplier` DECIMAL(10,2) NOT NULL DEFAULT 1.00,
                    `InputPricePerMillion` DECIMAL(10,4) NOT NULL DEFAULT 0.0000,
                    `OutputPricePerMillion` DECIMAL(10,4) NOT NULL DEFAULT 0.0000,
                    `IsEnabled` TINYINT(1) NOT NULL DEFAULT 1,
                    PRIMARY KEY (`Id`),
                    UNIQUE KEY `idx_aimodelwhitelist_modelid` (`ModelId`),
                    INDEX `idx_aimodelwhitelist_provider` (`Provider`),
                    INDEX `idx_aimodelwhitelist_enabled` (`IsEnabled`)
                  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;"
            ).ExecuteUpdate();

            // Add token balance fields and AI model preference to user table
            nhibernateSession.CreateSQLQuery(
                @"ALTER TABLE `user` 
                  ADD COLUMN `SubscriptionTokensBalance` INT NOT NULL DEFAULT 0,
                  ADD COLUMN `PaidTokensBalance` INT NOT NULL DEFAULT 0,
                  ADD COLUMN `PreferredAiModelId` VARCHAR(255) DEFAULT NULL;"
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
