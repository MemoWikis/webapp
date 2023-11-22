using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs242
{
    public static void Run(ISession nhibernateSession)  
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE User CHANGE subscriptionDuration EndDate DATETIME NULL;"
            ).ExecuteUpdate();
    }
}