namespace TrueOrFalse.Updates;

internal class UpdateToVs266
{
    public static void Run(
        PageRepository pageRepository,
        UserWritingRepo userWritingRepo,
        UserReadingRepo userReadingRepo)
    {
        PersonalTopicMigration.CreateOrAddPersonalTopicForUsersWithoutStartTopicId(
            pageRepository, userWritingRepo, userReadingRepo);
    }
}