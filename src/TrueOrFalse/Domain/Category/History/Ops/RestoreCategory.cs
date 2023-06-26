using System.Net.Mail;

public class RestoreCategory : IRegisterAsInstancePerLifetime
{
    private readonly int _sessionUserId;

    public RestoreCategory(SessionUser sessionUser)
    {
        _sessionUserId = sessionUser.UserId;
    }

    public void Run(int categoryChangeId, User author)
    {
        var categoryChange = Sl.CategoryChangeRepo.GetByIdEager(categoryChangeId);
        var historicCategory = categoryChange.ToHistoricCategory();
        var category = Sl.CategoryRepo.GetById(historicCategory.Id);
        var categoryCacheItem = EntityCache.GetCategory(category.Id);

        category.Name = historicCategory.Name;
        category.Content = historicCategory.Content;
        categoryCacheItem.Name = historicCategory.Name;
        categoryCacheItem.Content = historicCategory.Content;

        EntityCache.AddOrUpdate(categoryCacheItem);
        var authorSessionUserCacheItem = SessionUserCacheItem.CreateCacheItem(author);
        Sl.CategoryRepo.Update(category, authorSessionUserCacheItem, type: CategoryChangeType.Restore);

        NotifyAboutRestore(categoryChange);
    }

    private void NotifyAboutRestore(CategoryChange categoryChange)
    {
        var category = categoryChange.Category;
        var currentUser = Sl.UserRepo.GetById(_sessionUserId);
        var subject = $"Kategorie {category.Name} zurückgesetzt";
        var body = $"Die Kategorie '{category.Name}' wurde gerade zurückgesetzt.\n" +
                   $"Zurückgesetzt auf Revision: vom {categoryChange.DateCreated} (Id {categoryChange.Id})\n" +
                   $"Von Benutzer: {currentUser.Name} (Id {currentUser.Id})";

        SendEmail(Constants.MemuchoAdminUserId, subject, body);
        if (category.Creator != null && currentUser.Id != category.Creator.Id) 
            SendEmail(category.Creator.Id, subject, body);
    }

    private static void SendEmail(int receiverId, string subject, string body)
    {
        CustomMsg.Send(receiverId, subject, body);

        var user = Sl.UserRepo.GetById(receiverId);
        var mail = new MailMessage();
        mail.To.Add(user.EmailAddress);
        mail.From = new MailAddress(Settings.EmailFrom);
        mail.Subject = subject;
        mail.Body = body;
        global::SendEmail.Run(mail, MailMessagePriority.Low);
    }
}
