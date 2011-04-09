using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core.Registration
{
    public class SendRegistrationEmail
    {
        private readonly CreateEmailConfirmationLink _createEmailConfirmationLink;

        public SendRegistrationEmail(CreateEmailConfirmationLink createEmailConfirmationLink){
            _createEmailConfirmationLink = createEmailConfirmationLink;
        }

        private readonly SendEmail _sendEmail;

        public SendRegistrationEmail(SendEmail sendEmail)
        {
            _sendEmail = sendEmail;
        }

        public void Run(User user)
        {
            var mail = new MailMessage();
            mail.To.Add(user.EmailAddress);
            mail.From = new MailAddress(Settings.EmailDefaultFrom);

            var emailBody = new StringBuilder();
            emailBody.Append("Schön dass Du dabei bist.");
            emailBody.Append("Um Dein Benutzerkonto zu bestätigen,");
            emailBody.Append("folge bitte diesem Link:" + _createEmailConfirmationLink.Run(user));

            mail.Subject = "Willkommen bei True Or False";
            mail.Body = emailBody.ToString();
            
            _sendEmail.Run(mail);
        }
    }
}
