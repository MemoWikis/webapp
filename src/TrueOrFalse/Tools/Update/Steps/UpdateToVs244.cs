using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs244
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE User CHANGE stripeId StripeId VARCHAR(255);"
            ).ExecuteUpdate();
    }
}