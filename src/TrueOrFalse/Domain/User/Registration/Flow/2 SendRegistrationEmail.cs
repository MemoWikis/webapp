﻿using System.Net.Mail;
using System.Text;

public class SendRegistrationEmail
{
    public static void Run(User user)
    {
        var mail = new MailMessage();
        mail.To.Add(user.EmailAddress);
        mail.From = new MailAddress(Settings.EmailFrom);

        var emailBody = new StringBuilder();
        emailBody.AppendLine("Hallo " + user.Name + ",");
        emailBody.AppendLine("");
        emailBody.AppendLine("du hast dich gerade bei memucho registriert, wir freuen uns, dass du dabei bist!");
        emailBody.AppendLine("");
        emailBody.AppendLine("Um dein Benutzerkonto zu bestätigen, folge bitte diesem Link:");
        emailBody.AppendLine(CreateEmailConfirmationLink.Run(user));

        mail.Subject = "Willkommen bei memucho";
        mail.Body = emailBody.ToString();
            
        SendEmail.Run(mail);
    }
}