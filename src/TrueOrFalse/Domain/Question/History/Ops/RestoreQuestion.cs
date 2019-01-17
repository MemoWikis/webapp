using System.Net.Mail;

public class RestoreQuestion
{
    [RedirectToErrorPage_IfNotLoggedIn]
    public static void Run(int questionChangeId, User author)
    {
        var questionChange = Sl.QuestionChangeRepo.GetByIdEager(questionChangeId);
        var historicQuestion = questionChange.ToHistoricQuestion();
        // TODO FK author?
        //Sl.QuestionRepo.Update(historicQuestion, author);
        //Sl.QuestionRepo.Update(historicQuestion, merge: true);
        Sl.QuestionRepo.Merge(historicQuestion);

        NotifyAboutRestore(questionChange);
    }

    private static void NotifyAboutRestore(QuestionChange questionChange)
    {
        var question = questionChange.Question;
        var currentUser = Sl.UserRepo.GetById(Sl.CurrentUserId);
        var subject = $"Frage {question.Text} zurückgesetzt";
        var body = $"Die Frage '{question.Text}' wurde gerade zurückgesetzt.\n" +
                   $"Zurückgesetzt auf Revision: vom {questionChange.DateCreated} (Id {questionChange.Id})\n" +
                   $"Von Benutzer: {currentUser.Name} (Id {currentUser.Id})";

        SendEmail(Constants.MemuchoAdminUserId, subject, body);
        if (currentUser.Id != question.Creator.Id)
            SendEmail(question.Creator.Id, subject, body);
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
