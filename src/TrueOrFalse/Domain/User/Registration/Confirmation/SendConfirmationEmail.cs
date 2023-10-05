using System.Net.Mail;
using System.Text;

public class SendConfirmationEmail
{
    public static void Run(int userId)
    {
        var userRepo = Sl.UserRepo;
        userRepo.Flush();
        var user = Sl.UserRepo.GetById(userId);
        userRepo.Refresh(user);

        Run(user);
    }
    public static void Run(User user)
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

        mail.Subject = "memucho - Bestätigungslink für deine E-Mail-Adresse";
        mail.Body = emailBody.ToString();

        SendEmail.Run(mail, MailMessagePriority.High);
    }
}