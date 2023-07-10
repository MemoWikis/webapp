using System.Net.Mail;

public class RestoreQuestion : IRegisterAsInstancePerLifetime
{
    private readonly int _currentUserId;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly QuestionRepo _questionRepo;
    private readonly QuestionChangeRepo _questionChangeRepo;
    private readonly UserRepo _userRepo;
    private readonly MessageRepo _messageRepo;

    public RestoreQuestion(int currentUserId,
        JobQueueRepo jobQueueRepo,
        QuestionRepo questionRepo,
        QuestionChangeRepo questionChangeRepo,
        UserRepo userRepo,
        MessageRepo messageRepo)
    {
        _currentUserId = currentUserId;
        _jobQueueRepo = jobQueueRepo;
        _questionRepo = questionRepo;
        _questionChangeRepo = questionChangeRepo;
        _userRepo = userRepo;
        _messageRepo = messageRepo;
    }

    public void Run(int questionChangeId, User author)
    {
        var questionChange = _questionChangeRepo.GetByIdEager(questionChangeId);
        var historicQuestion = questionChange.ToHistoricQuestion();
        _questionRepo.Merge(historicQuestion);

        _questionChangeRepo.AddUpdateEntry(historicQuestion, _questionRepo);

        NotifyAboutRestore(questionChange);
    }

    private void NotifyAboutRestore(QuestionChange questionChange)
    {
        var question = questionChange.Question;
        var currentUser = _userRepo.GetById(_currentUserId);
        var subject = $"Frage {question.Text} zurückgesetzt";
        var body = $"Die Frage '{question.Text}' mit Id {question.Id} wurde gerade zurückgesetzt.\n" +
                   $"Zurückgesetzt auf Revision: vom {questionChange.DateCreated} (Id {questionChange.Id})\n" +
                   $"Von Benutzer: {currentUser.Name} (Id {currentUser.Id})";

        SendEmail(Constants.MemuchoAdminUserId, subject, body);
        if (question.Creator != null && currentUser.Id != question.Creator.Id)
            SendEmail(question.Creator.Id, subject, body);
    }

    private void SendEmail(int receiverId, string subject, string body)
    {
        CustomMsg.Send(receiverId, subject, body, _messageRepo, _userRepo);

        var user = _userRepo.GetById(receiverId);
        var mail = new MailMessage();
        mail.To.Add(user.EmailAddress);
        mail.From = new MailAddress(Settings.EmailFrom);
        mail.Subject = subject;
        mail.Body = body;
        global::SendEmail.Run(mail, _jobQueueRepo, MailMessagePriority.Low);
    }
}
