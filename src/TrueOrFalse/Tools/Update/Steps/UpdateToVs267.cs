using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs267
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"CREATE TABLE TopicOrder (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    TopicId INT NOT NULL,
                    ParentId INT NULL,
                    PreviousId INT NULL,
                    NextId INT NULL
                )"
            ).ExecuteUpdate();
    }
}