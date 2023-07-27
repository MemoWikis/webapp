using System.Net.Mail;

public class PasswordRecovery : IRegisterAsInstancePerLifetime
{
    private readonly PasswordRecoveryTokenRepository _tokenRepository;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly UserReadingRepo _userReadingRepo;

    public PasswordRecovery(PasswordRecoveryTokenRepository tokenRepository,
        JobQueueRepo jobQueueRepo, 
        UserReadingRepo userReadingRepo)
    {
        _tokenRepository = tokenRepository;
        _jobQueueRepo = jobQueueRepo;
        _userReadingRepo = userReadingRepo;
    }

    public PasswordRecoveryResult Run(string email)
    {
        if (IsEmailAddressAvailable.Yes(email, _userReadingRepo))
            return new PasswordRecoveryResult { EmailDoesNotExist = true, Success = false };

        try
        {
            var token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
            var passwortResetUrl = "https://memucho.de/Welcome/PasswordReset/" + token;

            _tokenRepository.Create(new PasswordRecoveryToken { Email = email, Token = token });
            SendEmail.Run(GetMailMessage(email, passwortResetUrl), _jobQueueRepo, _userReadingRepo, MailMessagePriority.High);
        }
        catch(Exception e)
        {
            Logg.r().Error(e, $"Error while trying to reset password for email: {email}");
            return new PasswordRecoveryResult { Success = false };
        }

        return new PasswordRecoveryResult { Success = true };
    }

    public PasswordRecoveryResult RunForNuxt(string email)
    {
        if (IsEmailAddressAvailable.Yes(email, _userReadingRepo))
            return new PasswordRecoveryResult { EmailDoesNotExist = true, Success = false };

        try
        {
            var token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
            var passwordResetUrl = "https://memucho.de/NeuesPasswort/" + token;

            _tokenRepository.Create(new PasswordRecoveryToken { Email = email, Token = token });
            SendEmail.Run(GetMailMessage(email, passwordResetUrl), _jobQueueRepo, _userReadingRepo, MailMessagePriority.High);
        }
        catch (Exception e)
        {
            Logg.r().Error(e, $"Error while trying to reset password for email: {email}");
            return new PasswordRecoveryResult { Success = false };
        }

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