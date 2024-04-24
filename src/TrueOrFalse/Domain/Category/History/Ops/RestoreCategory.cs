using System.Net.Mail;

public class RestoreCategory(
    SessionUser _sessionUser,
    CategoryRepository _categoryRepository,
    CategoryChangeRepo _categoryChangeRepo,
    JobQueueRepo _jobQueueRepo,
    UserReadingRepo _userReadingRepo,
    MessageRepo _messageRepo)
    : IRegisterAsInstancePerLifetime
{
    private readonly int _sessionUserId = _sessionUser.UserId;

    public void Run(int categoryChangeId, User author)
    {
        var categoryChange = _categoryChangeRepo.GetByIdEager(categoryChangeId);
        var historicCategory = categoryChange.ToHistoricCategory();
        var category = _categoryRepository.GetById(historicCategory.Id);
        var categoryCacheItem = EntityCache.GetCategory(category.Id);

        category.Name = historicCategory.Name;
        category.Content = historicCategory.Content;
        categoryCacheItem.Name = historicCategory.Name;
        categoryCacheItem.Content = historicCategory.Content;

        EntityCache.AddOrUpdate(categoryCacheItem);
        var authorSessionUserCacheItem = SessionUserCacheItem.CreateCacheItem(author);
        _categoryRepository
            .Update(category, authorSessionUserCacheItem.Id, type: CategoryChangeType.Restore);

        NotifyAboutRestore(categoryChange);
    }

    public void Run(int categoryChangeId, SessionUserCacheItem author)
    {
        var categoryChange = _categoryChangeRepo.GetByIdEager(categoryChangeId);
        var historicCategory = categoryChange.ToHistoricCategory();
        var category = _categoryRepository.GetById(historicCategory.Id);
        var categoryCacheItem = EntityCache.GetCategory(category.Id);

        category.Name = historicCategory.Name;
        category.Content = historicCategory.Content;
        categoryCacheItem.Name = historicCategory.Name;
        categoryCacheItem.Content = historicCategory.Content;

        EntityCache.AddOrUpdate(categoryCacheItem);
        _categoryRepository.Update(category, author.Id, type: CategoryChangeType.Restore);

        NotifyAboutRestore(categoryChange);
    }

    private void NotifyAboutRestore(CategoryChange categoryChange)
    {
        var category = categoryChange.Category;
        var currentUser = _userReadingRepo.GetById(_sessionUserId);
        var subject = $"Kategorie {category.Name} zurückgesetzt";
        var body = $"Die Kategorie '{category.Name}' wurde gerade zurückgesetzt.\n" +
                   $"Zurückgesetzt auf Revision: vom {categoryChange.DateCreated} (Id {categoryChange.Id})\n" +
                   $"Von Benutzer: {currentUser.Name} (Id {currentUser.Id})";

        SendEmail(Constants.MemuchoAdminUserId, subject, body);
        if (category.Creator != null && currentUser.Id != category.Creator.Id)
            SendEmail(category.Creator.Id, subject, body);
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