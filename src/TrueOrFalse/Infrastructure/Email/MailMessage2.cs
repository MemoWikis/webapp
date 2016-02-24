using System.Net.Mail;

public class MailMessage2 : MailMessage
{
    public string UserName;

    public MailMessage2(){}
    public MailMessage2(MailAddress from, MailAddress to) : base(from, to){}
    public MailMessage2(string from, string to) : base(from, to){}
    public MailMessage2(string from, string to, string subject, string body) : base(from, to, subject, body){}
}