namespace TrueOrFalse.Updates;

internal class UpdateToVs266
{
    public static void Run(
        CategoryRepository categoryRepository,
        UserWritingRepo userWritingRepo,
        UserReadingRepo userReadingRepo)
    {
        PersonalTopicMigration.CreateOrAddPersonalTopicForUsersWithoutStartTopicId(
            categoryRepository, userWritingRepo, userReadingRepo);
    }
}