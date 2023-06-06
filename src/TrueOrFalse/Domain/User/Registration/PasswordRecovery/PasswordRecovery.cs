using System.Net.Mail;

public class PasswordRecovery : IRegisterAsInstancePerLifetime
{
    private readonly PasswordRecoveryTokenRepository _tokenRepository;

    public PasswordRecovery(PasswordRecoveryTokenRepository tokenRepository)
    {
        _tokenRepository = tokenRepository;
    }

    public PasswordRecoveryResult Run(string email)
    {
        if (IsEmailAddressAvailable.Yes(email))
            return new PasswordRecoveryResult { TheEmailDoesNotExist = true, Success = false };

        var token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
        var passwortResetUrl = "https://memucho.de/Welcome/PasswordReset/" + token;

        _tokenRepository.Create(new PasswordRecoveryToken { Email = email, Token = token });
        SendEmail.Run(GetMailMessage(email, passwortResetUrl), MailMessagePriority.High);

        return new PasswordRecoveryResult { Success = true };
    }

    private MailMessage GetMailMessage(string emailAdresse, string passwortResetUrl)
    {
        var mailMessage = new MailMessage();
        mailMessage.To.Add(new MailAddress(emailAdresse));
        mailMessage.From = new MailAddress(Settings.EmailFrom);
        mailMessage.Subject =
            "Dein neues Passwort für memucho";
        mailMessage.Body = @"
Um ein neues Passwort zu setzen, folge diesem Link: {0}

Der Link ist 72 Stunden lang gültig.

Viele Grüße
Dein memucho-Team
".Replace("{0}", passwortResetUrl);

        return mailMessage;
    }
}