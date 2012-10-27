using System;
using System.Net.Mail;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core.Registration
{
    public class PasswordRecovery : IRegisterAsInstancePerLifetime
    {
        private readonly IsEmailAddressNotInUse _emailAddressInUse;
        private readonly SendEmail _sendMailMessage;
        private readonly PasswordRecoveryTokenRepository _tokenRepository;

        public PasswordRecovery(IsEmailAddressNotInUse emailAddressInUse, 
                                SendEmail sendMailMessage,
                                PasswordRecoveryTokenRepository tokenRepository)
        {
            _emailAddressInUse = emailAddressInUse;
            _sendMailMessage = sendMailMessage;
            _tokenRepository = tokenRepository;
        }

        public PasswordRecoveryResult Run(string email)
        {
            if (!_emailAddressInUse.Yes(email))
                return new PasswordRecoveryResult { TheEmailDoesNotExist = true, Success = false };

            var token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
            var passwortResetUrl = "http://bewerbung.apollo-online.de/home/passwort_reset/" + token;

            _tokenRepository.Create(new PasswordRecoveryToken{ Email = email, Token = token });
            _sendMailMessage.Run(GetMailMessage(email, passwortResetUrl));

            return new PasswordRecoveryResult { Success = true };
        }

        private MailMessage GetMailMessage(string emailAdresse, string passwortResetUrl)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(new MailAddress(emailAdresse));
            mailMessage.From = new MailAddress("team@richtig-oder-falsch.de");
            mailMessage.Subject =
                "Dein neues Passwort für TrueOrFalse";
            mailMessage.Body = @"
Um ein neues Passwort zu setzen, folge diesem Link: {0}

Der Link ist 72 Stunden lang gültig.

Viele Grüße
Dein Riofa Team
".Replace("{0}", passwortResetUrl);

            return mailMessage;
        }
    }
}