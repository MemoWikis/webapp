public class RestoreCategory
{

    public static void Run(int categoryChangeId, User author)
    {
        var categoryChange = Sl.CategoryChangeRepo.GetByIdEager(categoryChangeId);
        var historicCategory = categoryChange.ToHistoricCategory();
        Sl.CategoryRepo.Update(historicCategory, author);

        NotifyAboutRestore(categoryChange);
    }

    private static void NotifyAboutRestore(CategoryChange categoryChange)
    {
        var category = categoryChange.Category;
        var currentUser = Sl.UserRepo.GetById(Sl.CurrentUserId);
        var subject = $"Kategorie {category.Name} zurückgesetzt";
        var body = $"Die Kategorie '{category.Name}' wurde gerade zurückgesetzt.\n" +
                   $"Zurückgesetzt auf Revision: vom {categoryChange.DateCreated} (Id {categoryChange.Id})\n" +
                   $"Von Benutzer: {currentUser.Name} (Id {currentUser.Id})";

        CustomMsg.Send(category.Creator.Id, subject, body);
        CustomMsg.Send(Constants.MemuchoAdminUserId, subject, body);
        //CustomMsg.Send("Franziska", subject, body);
    }
}
