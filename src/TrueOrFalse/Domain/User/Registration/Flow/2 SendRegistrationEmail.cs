using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Registration
{
    public class SendRegistrationEmail : IRegisterAsInstancePerLifetime
    {
        private readonly CreateEmailConfirmationLink _createEmailConfirmationLink;

        public SendRegistrationEmail(CreateEmailConfirmationLink createEmailConfirmationLink,
                                     SendEmail sendEmail)
        {
            _createEmailConfirmationLink = createEmailConfirmationLink;
            _sendEmail = sendEmail;
        }

        private readonly SendEmail _sendEmail;

        public void Run(User user)
        {
            var mail = new MailMessage();
            mail.To.Add(user.EmailAddress);
            mail.From = new MailAddress("willkommen@memucho.de");

            var emailBody = new StringBuilder();
            emailBody.AppendLine("Schön dass du dabei bist.");
            emailBody.AppendLine("Um dein Benutzerkonto zu bestätigen,");
            emailBody.AppendLine("folge bitte diesem Link: " + _createEmailConfirmationLink.Run(user));

            mail.Subject = "Willkommen bei MEMuchO";
            mail.Body = emailBody.ToString();
            
            _sendEmail.Run(mail);
        }
    }
}
