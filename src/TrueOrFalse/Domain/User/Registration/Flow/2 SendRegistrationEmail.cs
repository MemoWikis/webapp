using System.Net.Mail;

public class SendRegistrationEmail
{
    public static void Run(User user, JobQueueRepo jobQueueRepo, UserReadingRepo userReadingRepo)
    {
        var mail = new MailMessage();
        mail.To.Add(user.EmailAddress);
        mail.From = new MailAddress(Settings.EmailFrom);

        mail.Subject = GetSubjectByUiLanguage(user.UiLanguage);

        mail.Body = GetBodyByUiLanguage(user.UiLanguage, user.Name, CreateEmailConfirmationLink.Run(user));

        SendEmail.Run(mail, jobQueueRepo, userReadingRepo, MailMessagePriority.High);
    }

    private static string GetSubjectByUiLanguage(string languageCode)
    {
        var language = LanguageExtensions.GetLanguage(languageCode);

        return language switch
        {
            Language.German => "Willkommen bei MemoWikis",
            Language.French => "Bienvenue sur MemoWikis",
            Language.Spanish => "Bienvenido(a) a MemoWikis",
            // Default to English
            _ => "Welcome to MemoWikis"
        };
    }

    private static string GetBodyByUiLanguage(string languageCode, string userName, string confirmationLink)
    {
        var language = LanguageExtensions.GetLanguage(languageCode);

        // Choose the template and format it
        switch (language)
        {
            case Language.German:
                return string.Format(RegistrationEmailBodyDe, userName, confirmationLink);
            case Language.French:
                return string.Format(RegistrationEmailBodyFr, userName, confirmationLink);
            case Language.Spanish:
                return string.Format(RegistrationEmailBodyEs, userName, confirmationLink);
            // Default: English
            default:
                return string.Format(RegistrationEmailBodyEn, userName, confirmationLink);
        }
    }


    private static string RegistrationEmailBodyDe = @"
        Hallo {0},
        
        du hast dich gerade bei MemoWikis registriert, wir freuen uns, dass du dabei bist!
        
        Um dein Benutzerkonto zu bestätigen, folge bitte diesem Link:
        {1}
        
        Viele Grüße,
        Dein MemoWikis-Team
        ";

    private static string RegistrationEmailBodyEn = @"
        Hello {0},
        
        You’ve just registered at MemoWikis, and we’re excited to have you onboard!
        
        To confirm your account, please follow this link:
        {1}
        
        Best regards,
        The MemoWikis Team
        ";

    private static string RegistrationEmailBodyFr = @"
        Bonjour {0},
        
        Vous venez de vous inscrire sur MemoWikis et nous sommes ravis de vous accueillir !
        
        Pour confirmer votre compte, veuillez suivre ce lien :
        {1}
        
        Bien cordialement,
        L'équipe MemoWikis
        ";

    private static string RegistrationEmailBodyEs = @"
        Hola {0},
        
        Te acabas de registrar en MemoWikis y estamos muy contentos de tenerte con nosotros.
        
        Para confirmar tu cuenta, sigue este enlace:
        {1}
        
        Un cordial saludo,
        El equipo de MemoWikis
        ";
}
