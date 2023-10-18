using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs266
{
    public static void Run()
    {
        PersonalTopicMigration.CreateOrAddPersonalTopicForUsersWithoutStartTopicId();
    }
}