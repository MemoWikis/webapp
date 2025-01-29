using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;

public class PasswordRecovery : IRegisterAsInstancePerLifetime
{
    private readonly PasswordRecoveryTokenRepository _tokenRepository;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PasswordRecovery(PasswordRecoveryTokenRepository tokenRepository,
        JobQueueRepo jobQueueRepo,
        UserReadingRepo userReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _tokenRepository = tokenRepository;
        _jobQueueRepo = jobQueueRepo;
        _userReadingRepo = userReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public PasswordRecoveryResult Run(string email)
    {
        if (IsEmailAddressAvailable.Yes(email, _userReadingRepo))
            return new PasswordRecoveryResult { EmailDoesNotExist = true, Success = false };

        try
        {
            var token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
            var passwortResetUrl = Settings.BaseUrl + "/Welcome/PasswordReset/" + token;

            _tokenRepository.Create(new PasswordRecoveryToken { Email = email, Token = token });
            SendEmail.Run(GetMailMessage(email, passwortResetUrl), _jobQueueRepo, _userReadingRepo, MailMessagePriority.High);
        }
        catch (Exception e)
        {
            Logg.r.Error(e, $"Error while trying to reset password for email: {email}");
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
            var passwordResetUrl = Settings.BaseUrl + "/NeuesPasswort/" + token;

            _tokenRepository.Create(new PasswordRecoveryToken { Email = email, Token = token });
            SendEmail.Run(GetMailMessage(email, passwordResetUrl), _jobQueueRepo, _userReadingRepo, MailMessagePriority.High);
        }
        catch (Exception e)
        {
            Logg.r.Error(e, $"Error while trying to reset password for email: {email}");
            return new PasswordRecoveryResult { Success = false };
        }

        return new PasswordRecoveryResult { Success = true };
    }

    private MailMessage GetMailMessage(string email, string passwordResetUrl)
    {
        var mailMessage = new MailMessage();
        mailMessage.To.Add(new MailAddress(email));
        mailMessage.From = new MailAddress(Settings.EmailFrom);
        mailMessage.Subject =
            "Dein neues Passwort für MemoWikis";
        mailMessage.Body = @"
Um ein neues Passwort zu setzen, folge diesem Link: {0}

Der Link ist 72 Stunden lang gültig.

Viele Grüße
Dein MemoWikis-Team
".Replace("{0}", passwordResetUrl);

        return mailMessage;
    }
}