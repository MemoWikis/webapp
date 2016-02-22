using System.IO;
using System.Net.Mail;
using RazorEngine;

public class HtmlMessage
{
    public static void Send(MailMessage mailMessage)
    {
        var parsedTemplate = Razor.Parse(
            File.ReadAllText(PathTo.EmailTemplate()),
            new HtmlMessageModel {Content = mailMessage.Body});

        mailMessage.Body = parsedTemplate;
        mailMessage.IsBodyHtml = true;

        SendEmail.Run(mailMessage);
    }
}