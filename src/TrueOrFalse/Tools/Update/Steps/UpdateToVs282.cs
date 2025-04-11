using NHibernate;

namespace TrueOrFalse.Updates;

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
                        PRIMARY KEY (`Id`)
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
