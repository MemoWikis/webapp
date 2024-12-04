using System.Net.Mail;

public class RestorePage(
    SessionUser _sessionUser,
    PageRepository pageRepository,
    PageChangeRepo pageChangeRepo,
    JobQueueRepo _jobQueueRepo,
    UserReadingRepo _userReadingRepo,
    MessageRepo _messageRepo)
    : IRegisterAsInstancePerLifetime
{
    private readonly int _sessionUserId = _sessionUser.UserId;

    public void Run(int pageChangeId, User author)
    {
        var pageChange = pageChangeRepo.GetByIdEager(pageChangeId);
        var historicPage = pageChange.ToHistoryPage();
        var page = pageRepository.GetById(historicPage.Id);
        var pageCacheItem = EntityCache.GetPage(page.Id);

        page.Name = historicPage.Name;
        page.Content = historicPage.Content;
        pageCacheItem.Name = historicPage.Name;
        pageCacheItem.Content = historicPage.Content;

        EntityCache.AddOrUpdate(pageCacheItem);
        pageRepository.Update(page, author.Id, type: PageChangeType.Restore);

        NotifyAboutRestore(pageChange);
    }

    public void Run(int pageChangeId, ExtendedUserCacheItem author)
    {
        var pageChange = pageChangeRepo.GetByIdEager(pageChangeId);
        var historicPage = pageChange.ToHistoryPage();
        var page = pageRepository.GetById(historicPage.Id);
        var pageCacheItem = EntityCache.GetPage(page.Id);

        page.Name = historicPage.Name;
        page.Content = historicPage.Content;
        pageCacheItem.Name = historicPage.Name;
        pageCacheItem.Content = historicPage.Content;

        EntityCache.AddOrUpdate(pageCacheItem);
        pageRepository.Update(page, author.Id, type: PageChangeType.Restore);

        NotifyAboutRestore(pageChange);
    }

    private void NotifyAboutRestore(PageChange pageChange)
    {
        var page = pageChange.Page;
        var currentUser = _userReadingRepo.GetById(_sessionUserId);
        var subject = $"Kategorie {page.Name} zurückgesetzt";
        var body = $"Die Kategorie '{page.Name}' wurde gerade zurückgesetzt.\n" +
                   $"Zurückgesetzt auf Revision: vom {pageChange.DateCreated} (Id {pageChange.Id})\n" +
                   $"Von Benutzer: {currentUser.Name} (Id {currentUser.Id})";

        SendEmail(Constants.MemuchoAdminUserId, subject, body);
        if (page.Creator != null && currentUser.Id != page.Creator.Id)
            SendEmail(page.Creator.Id, subject, body);
    }

    private void SendEmail(int receiverId, string subject, string body)
    {
        CustomMsg.Send(receiverId, subject, body, _messageRepo, _userReadingRepo);

        var user = _userReadingRepo.GetById(receiverId);
        var mail = new MailMessage();
        mail.To.Add(user.EmailAddress);
        mail.From = new MailAddress(Settings.EmailFrom);
        mail.Subject = subject;
        mail.Body = body;
        global::SendEmail.Run(mail, _jobQueueRepo, _userReadingRepo, MailMessagePriority.Low);
    }
}