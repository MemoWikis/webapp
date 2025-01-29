using System.Net.Mail;
using System.Text;

public class SendConfirmationEmail
{
    public static void Run(int userId, JobQueueRepo jobQueueRepo, UserReadingRepo userReadingRepo)
    {
        userReadingRepo.Flush();
        var user = userReadingRepo.GetById(userId);
        userReadingRepo.Refresh(user);

        Run(user, jobQueueRepo, userReadingRepo);
    }
    public static void Run(User user, JobQueueRepo jobQueueRepo, UserReadingRepo userReadingRepo)
    {
        var mail = new MailMessage();
        mail.To.Add(user.EmailAddress);
        mail.From = new MailAddress(Settings.EmailFrom);

        var emailBody = new StringBuilder();
        emailBody.AppendLine("Hallo " + user.Name + ",");
        emailBody.AppendLine("");
        emailBody.AppendLine("Wie gewünscht ist hier Dein Link zur E-Mail-Bestätigung:");
        emailBody.AppendLine("");
        emailBody.AppendLine(CreateEmailConfirmationLink.Run(user));
        emailBody.AppendLine("");
        emailBody.AppendLine("Klicke einfach darauf, um Deine E-Mail-Adresse zu bestätigen. Falls Du weitere Fragen hast, lass es uns wissen!");

        mail.Subject = "memoWikis - Bestätigungslink für deine E-Mail-Adresse";
        mail.Body = emailBody.ToString();

        SendEmail.Run(mail, jobQueueRepo, userReadingRepo, MailMessagePriority.High);
    }
}