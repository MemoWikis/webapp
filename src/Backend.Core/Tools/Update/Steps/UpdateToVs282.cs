using NHibernate;

internal class UpdateToVs282
{
    public static void Run(ISession nhibernateSession)
    {
        using var transaction = nhibernateSession.BeginTransaction();
        try
        {
            nhibernateSession.CreateSQLQuery(
                    @"CREATE TABLE `shares` (
                        `Id` INT NOT NULL AUTO_INCREMENT,
                        `PageId` INT NOT NULL,
                        `UserId` INT NULL,
                        `Token` VARCHAR(255) NOT NULL,
                        `Permission` INT NOT NULL,
                        `GrantedBy` INT NOT NULL,
                        PRIMARY KEY (`Id`),
                        INDEX `idx_shares_pageid` (`PageId`),
                        INDEX `idx_shares_userid` (`UserId`),
                        INDEX `idx_shares_token` (`Token`)
                      ) ENGINE=InnoDB DEFAULT CHARSET=utf8;")
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
