using NHibernate;

internal class UpdateToVs278
{
    public static void Run(ISession nhibernateSession)
    {
        using (var transaction = nhibernateSession.BeginTransaction())
        {
            try
            {
                // Single SQL query that updates Name, Description, and Content
                // for all rows in the 'page' table. 
                // We chain multiple REPLACE calls to handle all replacements at once.
                nhibernateSession.CreateSQLQuery(
                    @"UPDATE `page`
                        SET 
                           `Name` = REPLACE(
                                      REPLACE(
                                        REPLACE(`Name`, 'Memucho', 'memoWikis'),
                                      'memucho.de', 'memowikis.net'),
                                    'memucho', 'memoWikis'),

                           `Description` = REPLACE(
                                            REPLACE(
                                              REPLACE(`Description`, 'Memucho', 'memoWikis'),
                                            'memucho.de', 'memowikis.net'),
                                          'memucho', 'memoWikis'),

                           `Content` = REPLACE(
                                        REPLACE(
                                          REPLACE(`Content`, 'Memucho', 'memoWikis'),
                                        'memucho.de', 'memowikis.net'),
                                      'memucho', 'memoWikis');"
                                    ).ExecuteUpdate();

                // Commit if everything goes well
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Roll back changes if something failed
                transaction.Rollback();
                Console.Error.WriteLine($"An error occurred during the update: {ex.Message}");
                Console.Error.WriteLine(ex.StackTrace);
                // your logging method
                Log.Error($"An error occurred during the update: {ex.Message}", ex);

                throw;
            }
        }
    }
}
