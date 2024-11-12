public class PersonalPageMigration
{
    public static void CreateOrAddPersonalPageForUsersWithoutStartPageId(
        PageRepository pageRepository,
        UserWritingRepo userWritingRepo,
        UserReadingRepo userReadingRepo)
    {
        var users = userReadingRepo.GetAll();
        var allPages = pageRepository.GetAll();

        foreach (var user in users)
        {
            if (user.StartPageId <= 0)
            {
                Logg.r.Information("PersonalPageMigration - Start Migration for User: {userId}",
                    user.Id);
                var firstPage = allPages.FirstOrDefault(c => c.Creator == user);

                if (firstPage != null && firstPage.Name.Contains("Wiki"))
                {
                    user.StartPageId = firstPage.Id;
                    Logg.r.Information(
                        "PersonalPageMigration - User: {userId}, PageAdded: {pageId}", user.Id,
                        firstPage.Id);
                }
                else
                {
                    var newPage = PersonalPage.GetPersonalCategory(user, pageRepository);
                    pageRepository.CreateOnlyDb(newPage);
                    user.StartPageId = newPage.Id;
                    Logg.r.Information(
                        "PersonalPageMigration - User: {userId}, PageCreated: {pageId}", user.Id,
                        newPage.Id);
                }

                userWritingRepo.UpdateOnlyDb(user);
                Logg.r.Information("PersonalPageMigration - End Migration for User: {userId}",
                    user.Id);
            }
        }
    }
}