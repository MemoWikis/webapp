using System.Net.Mail;

public class RestoreCategory
{
    [RedirectToErrorPage_IfNotLoggedIn]
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

        SendEmail(Constants.MemuchoAdminUserId, subject, body);
        if (currentUser.Id != category.Creator.Id) 
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
        global::SendEmail.Run(mail);
    }
}
