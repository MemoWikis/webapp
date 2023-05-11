using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs244
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE User CHANGE stripeId StripeId VARCHAR(255);"
            ).ExecuteUpdate();
    }
}