using System.Net.Mail;

public class PasswordRecovery(
    PasswordRecoveryTokenRepository _tokenRepository,
    JobQueueRepo _jobQueueRepo,
    UserReadingRepo _userReadingRepo)
    : IRegisterAsInstancePerLifetime
{
    public PasswordRecoveryResult Run(string email)
    {
        if (IsEmailAddressAvailable.Yes(email, _userReadingRepo))
            return new PasswordRecoveryResult { EmailDoesNotExist = true, Success = false };

        try
        {
            var token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
            var passwordResetUrl = Settings.BaseUrl + "/NewPassword/" + token;

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
        var user = _userReadingRepo.GetByEmail(email);
        var language = LanguageExtensions.GetLanguage(user.UiLanguage);

        var mailMessage = new MailMessage
        {
            To = { new MailAddress(email) },
            From = new MailAddress(Settings.EmailFrom),
            Subject = GetSubjectByUiLanguage(language),
            Body = GetBodyByUiLanguage(language, passwordResetUrl)
        };

        return mailMessage;
    }

    /// <summary>
    /// Returns the subject line in the chosen language.
    /// </summary>
    private static string GetSubjectByUiLanguage(Language? language)
    {
        return language switch
        {
            Language.German => "Dein neues Passwort für memoWikis",
            Language.French => "Votre nouveau mot de passe pour memoWikis",
            Language.Spanish => "Tu nueva contraseña para memoWikis",
            _ => "Your new password for memoWikis" // English default
        };
    }

    /// <summary>
    /// Returns the body text in the chosen language, replacing {0} with the reset URL.
    /// </summary>
    private static string GetBodyByUiLanguage(Language? language, string resetUrl)
    {
        switch (language)
        {
            case Language.German:
                return string.Format(PasswordRecoveryBodyDe, resetUrl);
            case Language.French:
                return string.Format(PasswordRecoveryBodyFr, resetUrl);
            case Language.Spanish:
                return string.Format(PasswordRecoveryBodyEs, resetUrl);
            default: // English
                return string.Format(PasswordRecoveryBodyEn, resetUrl);
        }
    }

    // --------------------------------------------------
    // Translation Templates
    // --------------------------------------------------

    // German
    private static string PasswordRecoveryBodyDe = @"
        Um ein neues Passwort zu setzen, folge diesem Link: {0}
        
        Der Link ist 72 Stunden lang gültig.
        
        Viele Grüße
        Dein memoWikis-Team
        ";

    // English
    private static string PasswordRecoveryBodyEn = @"
        To set a new password, follow this link: {0}
        
        This link is valid for 72 hours.
        
        Best regards,
        Your memoWikis Team
        ";

    // French
    private static string PasswordRecoveryBodyFr = @"
        Pour définir un nouveau mot de passe, suivez ce lien : {0}
        
        Ce lien est valide pendant 72 heures.
        
        Cordialement,
        L'équipe memoWikis
        ";

    // Spanish
    private static string PasswordRecoveryBodyEs = @"
        Para establecer una nueva contraseña, sigue este enlace: {0}
        
        Este enlace es válido durante 72 horas.
        
        Saludos,
        El equipo de memoWikis
        ";
}
