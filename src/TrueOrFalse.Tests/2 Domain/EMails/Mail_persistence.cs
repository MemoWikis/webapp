
using System.Net.Mail;
using NUnit.Framework;

namespace TrueOrFalse.Tests.Persistence
{
    class Mail_persistence
    {
        [Test]
        public void SendTestMail()
        {
            var mail = new MailMessage(Settings.EmailFrom,
                Settings.EmailToMemucho,
                    "TestEmail",
                    "If you receive this mail, the test was successful.");

            SendEmail.Run(mail, MailMessagePriority.Medium);
        }
    }
}
