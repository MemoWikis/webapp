    using System.Net.Mail;

    public class MailMessageJob
    {
        public MailMessage MailMessage;
        public MailMessagePriority Priority;
    }
    public enum MailMessagePriority
    {
        Low = 0,
        Medium = 5,
        High = 10
    }