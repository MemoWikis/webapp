using System.Net.Mail;

public class RestoreQuestion : IRegisterAsInstancePerLifetime
{
    private readonly int _currentUserId;
    private readonly JobQueueRepo _jobQueueRepo;

    public RestoreQuestion(int currentUserId, JobQueueRepo jobQueueRepo)
    {
        _currentUserId = currentUserId;
        _jobQueueRepo = jobQueueRepo;
    }

    public void Run(int questionChangeId, User author)
    {
        var questionChange = Sl.QuestionChangeRepo.GetByIdEager(questionChangeId);
        var historicQuestion = questionChange.ToHistoricQuestion();
        Sl.QuestionRepo.Merge(historicQuestion);

        Sl.QuestionChangeRepo.AddUpdateEntry(historicQuestion);

        NotifyAboutRestore(questionChange);
    }

    private void NotifyAboutRestore(QuestionChange questionChange)
    {
        var question = questionChange.Question;
        var currentUser = Sl.UserRepo.GetById(_currentUserId);
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
        CustomMsg.Send(receiverId, subject, body);

        var user = Sl.UserRepo.GetById(receiverId);
        var mail = new MailMessage();
        mail.To.Add(user.EmailAddress);
        mail.From = new MailAddress(Settings.EmailFrom);
        mail.Subject = subject;
        mail.Body = body;
        global::SendEmail.Run(mail, _jobQueueRepo, MailMessagePriority.Low);
    }
}
