using System.Net.Mail;

public class NotifyUserPerEmail(
    JobQueueRepo _jobQueueRepo,
    UserReadingRepo _userReadingRepo)
    : IRegisterAsInstancePerLifetime
{
    public NotifyUserPerEmail Run(int userId, int pageId, SharePermission permission, string? customMessage = "")
    {
        try
        {
            var user = EntityCache.GetUserById(userId);
            if (user == null || string.IsNullOrEmpty(user.EmailAddress))
                return this;

            var page = EntityCache.GetPage(pageId);
            if (page == null)
                return this;

            var pageUrl = Settings.BaseUrl + "/" + UriSanitizer.Run(page.Name) + "/" + pageId;

            var language = LanguageExtensions.GetLanguage(user.UiLanguage);
            var mailMessage = GetMailMessage(user.EmailAddress, page, pageUrl, language, permission, customMessage);

            SendEmail.Run(mailMessage, _jobQueueRepo, _userReadingRepo, MailMessagePriority.High);
        }
        catch (Exception e)
        {
            Logg.r.Error(e, $"Error while sending sharing notification to userId: {userId} for pageId: {pageId}");
        }

        return this;
    }

    private MailMessage GetMailMessage(string email, PageCacheItem page, string pageUrl, Language? language, SharePermission permission, string? customMessage)
    {
        bool isEditPermission = permission == SharePermission.Edit || permission == SharePermission.EditWithChildren;

        var mailMessage = new MailMessage
        {
            To = { new MailAddress(email) },
            From = new MailAddress(Settings.EmailFrom),
            Subject = GetSubjectByUiLanguage(language, page.Name, isEditPermission),
            Body = GetBodyByUiLanguage(language, page.Name, pageUrl, page.Creator.Name, isEditPermission, customMessage)
        };

        return mailMessage;
    }

    private static string GetSubjectByUiLanguage(Language? language, string pageName, bool isEditPermission)
    {
        if (isEditPermission)
        {
            return language switch
            {
                Language.German => $"Du wurdest eingeladen, \"{pageName}\" zu bearbeiten",
                Language.French => $"Tu as été invité(e) à collaborer sur \"{pageName}\"",
                Language.Spanish => $"Has sido invitado a colaborar en \"{pageName}\"",
                _ => $"You've been invited to collaborate on \"{pageName}\""
            };
        }
        else
        {
            return language switch
            {
                Language.German => $"Du erhältst Zugriff auf \"{pageName}\"",
                Language.French => $"Tu as reçu l'accès à \"{pageName}\"",
                Language.Spanish => $"Has recibido acceso a \"{pageName}\"",
                _ => $"You've been granted access to \"{pageName}\""
            };
        }
    }

    private static string GetBodyByUiLanguage(Language? language, string pageName, string pageUrl, string creatorName, bool isEditPermission, string? customMessage)
    {
        var customMessageText = !string.IsNullOrEmpty(customMessage)
            ? $"\n\n\"{customMessage}\"\n\n"
            : "\n\n";

        if (isEditPermission)
        {
            switch (language)
            {
                case Language.German:
                    return string.Format(SharingInviteEditBodyDe, pageName, customMessageText, pageUrl, creatorName);
                case Language.French:
                    return string.Format(SharingInviteEditBodyFr, pageName, customMessageText, pageUrl, creatorName);
                case Language.Spanish:
                    return string.Format(SharingInviteEditBodyEs, pageName, customMessageText, pageUrl, creatorName);
                default:
                    return string.Format(SharingInviteEditBodyEn, pageName, customMessageText, pageUrl, creatorName);
            }
        }
        else
        {
            switch (language)
            {
                case Language.German:
                    return string.Format(SharingInviteViewBodyDe, pageName, customMessageText, pageUrl, creatorName);
                case Language.French:
                    return string.Format(SharingInviteViewBodyFr, pageName, customMessageText, pageUrl, creatorName);
                case Language.Spanish:
                    return string.Format(SharingInviteViewBodyEs, pageName, customMessageText, pageUrl, creatorName);
                default:
                    return string.Format(SharingInviteViewBodyEn, pageName, customMessageText, pageUrl, creatorName);
            }
        }
    }

    // --------------------------------------------------
    // Edit Collaboration Templates
    // --------------------------------------------------

    private static string SharingInviteEditBodyDe = @"
        Du wurdest eingeladen, an der Seite ""{0}"" mitzuarbeiten.{1}
        Klicke auf diesen Link, um zur Seite zu gelangen:
        {2}
        
        Viele Grüße
        {3} und das memoWikis-Team
        ";

    private static string SharingInviteEditBodyEn = @"
        You have been invited to collaborate on the page ""{0}"".{1}
        Click this link to access the page:
        {2}
        
        Best regards,
        {3} and the memoWikis Team
        ";

    private static string SharingInviteEditBodyFr = @"
        Tu as été invité(e) à collaborer sur la page ""{0}"".{1}
        Clique sur ce lien pour accéder à la page:
        {2}
        
        Cordialement,
        {3} et l'équipe memoWikis
        ";

    private static string SharingInviteEditBodyEs = @"
        Has sido invitado a colaborar en la página ""{0}"".{1}
        Haz clic en este enlace para acceder a la página:
        {2}
        
        Saludos,
        {3} y el equipo de memoWikis
        ";

    // --------------------------------------------------
    // View-only Templates
    // --------------------------------------------------

    private static string SharingInviteViewBodyDe = @"
        Dir wurde Zugriff auf die Seite ""{0}"" gewährt.{1}
        Klicke auf diesen Link, um zur Seite zu gelangen:
        {2}
        
        Viele Grüße
        {3} und das memoWikis-Team
        ";

    private static string SharingInviteViewBodyEn = @"
        You have been granted access to view the page ""{0}"".{1}
        Click this link to access the page:
        {2}
        
        Best regards,
        {3} and the memoWikis Team
        ";

    private static string SharingInviteViewBodyFr = @"
        Tu as reçu l'accès pour voir la page ""{0}"".{1}
        Clique sur ce lien pour accéder à la page:
        {2}
        
        Cordialement,
        {3} et l'équipe memoWikis
        ";

    private static string SharingInviteViewBodyEs = @"
        Has recibido acceso para ver la página ""{0}"".{1}
        Haz clic en este enlace para acceder a la página:
        {2}
        
        Saludos,
        {3} y el equipo de memoWikis
        ";
}
