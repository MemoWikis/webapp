    using System.Net.Mail;

    public class MailMessageJob
    {
        public MailMessage MailMessage;
        public MailMessagePriority Priority;
    }
    public enum MailMessagePriority
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Immediately = 3,
    }