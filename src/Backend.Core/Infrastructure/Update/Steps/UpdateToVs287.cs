using NHibernate;

internal class UpdateToVs287
{
    public static void Run(ISession nhibernateSession)
    {
        using var transaction = nhibernateSession.BeginTransaction();

        try
        {
            nhibernateSession.CreateSQLQuery(
                @"CREATE TABLE `userskill` (
                    `Id` INT NOT NULL AUTO_INCREMENT,
                    `UserId` INT NOT NULL,
                    `PageId` INT NOT NULL,
                    `EvaluationJson` TEXT,
                    `AddedAt` DATETIME NOT NULL,
                    `LastUpdatedAt` DATETIME NULL,
                    `DateCreated` DATETIME NOT NULL,
                    `DateModified` DATETIME NOT NULL,
                    PRIMARY KEY (`Id`),
                    UNIQUE KEY `idx_userskill_user_page` (`UserId`, `PageId`),
                    INDEX `idx_userskill_userid` (`UserId`),
                    INDEX `idx_userskill_pageid` (`PageId`),
                    INDEX `idx_userskill_addedat` (`AddedAt`)
                  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;"
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
