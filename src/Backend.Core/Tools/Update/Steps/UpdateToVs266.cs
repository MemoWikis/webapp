namespace TrueOrFalse.Updates;

internal class UpdateToVs266
{
    public static void Run(
        PageRepository pageRepository,
        UserWritingRepo userWritingRepo,
        UserReadingRepo userReadingRepo)
    {
        PersonalPageMigration.CreateOrAddPersonalPageForUsersWithoutStartPageId(
            pageRepository, userWritingRepo, userReadingRepo);
    }
}