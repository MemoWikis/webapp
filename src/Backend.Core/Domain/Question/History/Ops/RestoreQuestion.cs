using System.Net.Mail;

public class RestoreQuestion : IRegisterAsInstancePerLifetime
{
    private readonly int _currentUserId;
    private readonly JobQueueRepo _jobQueueRepo;
  
    private readonly QuestionChangeRepo _questionChangeRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly MessageRepo _messageRepo;
    private readonly QuestionWritingRepo _questionWritingRepo;

    public RestoreQuestion(int currentUserId,
        JobQueueRepo jobQueueRepo,
        QuestionChangeRepo questionChangeRepo,
        UserReadingRepo userReadingRepo,
        MessageRepo messageRepo,
        QuestionWritingRepo questionWritingRepo)
    {
        _currentUserId = currentUserId;
        _jobQueueRepo = jobQueueRepo;
        _questionChangeRepo = questionChangeRepo;
        _userReadingRepo = userReadingRepo;
        _messageRepo = messageRepo;
        _questionWritingRepo = questionWritingRepo;
    }

    public void Run(int questionChangeId, User author)
    {
        var questionChange = _questionChangeRepo.GetByIdEager(questionChangeId);
        var historicQuestion = questionChange.ToHistoricQuestion();
        _questionWritingRepo.Merge(historicQuestion);
        _questionChangeRepo.AddUpdateEntry(historicQuestion);
        NotifyAboutRestore(questionChange);
    }

    private void NotifyAboutRestore(QuestionChange questionChange)
    {
        var question = questionChange.Question;
        var currentUser = _userReadingRepo.GetById(_currentUserId);
        var subject = $"Frage {question.Text} zurückgesetzt";
        var body = $"Die Frage '{question.Text}' mit Id {question.Id} wurde gerade zurückgesetzt.\n" +
                   $"Zurückgesetzt auf Revision: vom {questionChange.DateCreated} (Id {questionChange.Id})\n" +
                   $"Von Benutzer: {currentUser.Name} (Id {currentUser.Id})";

        SendEmail(Constants.MemoWikisAdminUserId, subject, body);
        if (question.Creator != null && currentUser.Id != question.Creator.Id)
            SendEmail(question.Creator.Id, subject, body);
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
