public class WelcomeMsg
{
    public static void Send(int receiverId, UserReadingRepo userReadingRepo, MessageRepo messageRepo)
    {
        var user = MessageUtils.LoadUser(receiverId, userReadingRepo);
        Send(user, messageRepo);
    }

    public static void Send(User user, MessageRepo messageRepo)
    {
        var subject = GetSubjectByUiLanguage(user.UiLanguage);

        var body = GetWelcomeMessageByUiLanguage(user.UiLanguage, user.Name, Settings.EmailToMemoWikis);

        messageRepo.Create(new Message
        {
            ReceiverId = user.Id,
            Subject = subject,
            Body = body,
            MessageType = MessageTypes.Welcome
        });
    }

    private static string GetWelcomeMessageByUiLanguage(string languageCode, string userName, string email)
    {
        var language = LanguageExtensions.GetLanguage(languageCode);

        switch (language)
        {
            case Language.German:
                return string.Format(WelcomeMessageDe, userName, email);
            case Language.French:
                return string.Format(WelcomeMessageFr, userName, email);
            case Language.Spanish:
                return string.Format(WelcomeMessageEs, userName, email);
            case Language.English:
            default:
                return string.Format(WelcomeMessageEn, userName, email);
        }
    }

    private static string GetSubjectByUiLanguage(string languageCode)
    {
        var language = LanguageExtensions.GetLanguage(languageCode);
        switch (language)
        {
            case Language.German:
                return "Willkommen bei memoWikis";
            case Language.French:
                return "Bienvenue sur memoWikis";
            case Language.Spanish:
                return "Bienvenido(a) a memoWikis";
            case Language.English:
            default:
                return "Welcome to memoWikis";
        }
    }

    public static string WelcomeMessageDe = @"
        <p>Hallo {0},</p>
        <p>wir begrüßen dich herzlich bei memoWikis!</p>
        
        <p>memoWikis ist in der Beta-Phase, es gibt noch viel zu tun und wir haben viel vor. 
        Du kannst uns unterstützen, indem du deinen Freunden von uns erzählst und Fehler 
        und Verbesserungsvorschläge an uns weiterleitest. Solltest du irgendwelche Fragen 
        haben, helfen wir dir gerne.</p>
        
        <p>Viele Grüße,<br>
        Robert</p>
        <p style='font-size: 12px; margin-top: 20px'>E-Mail: {1} | Telefon: +49-178 186 68 48 (Robert)</p>
        ";

    public static string WelcomeMessageEn = @"
        <p>Hello {0},</p>
        <p>Welcome to memoWikis!</p>
        
        <p>memoWikis is currently in its beta phase, and there’s still much to do. 
        You can help by telling your friends about us and sending us any bugs or 
        improvement suggestions. If you have any questions, we’re here to help!</p>
        
        <p>Best regards,<br>
        Robert</p>
        <p style='font-size: 12px; margin-top: 20px'>Email: {1} | Phone: +49-178 186 68 48 (Robert)</p>
        ";

    public static string WelcomeMessageFr = @"
        <p>Bonjour {0},</p>
        <p>Bienvenue sur memoWikis&nbsp;!</p>
        
        <p>memoWikis est en phase bêta et il reste beaucoup à faire. 
        Vous pouvez nous aider en parlant de nous à vos amis et en nous 
        faisant part de tout bug ou suggestion d’amélioration. Si vous avez 
        la moindre question, nous sommes là pour vous aider&nbsp;!</p>
        
        <p>Bien cordialement,<br>
        Robert</p>
        <p style='font-size: 12px; margin-top: 20px'>E-mail&nbsp;: {1} | Téléphone&nbsp;: +49-178 186 68 48 (Robert)</p>
        ";

    public static string WelcomeMessageEs = @"
        <p>Hola {0},</p>
        <p>¡Bienvenido(a) a memoWikis!</p>
        
        <p>memoWikis está en fase beta y todavía hay mucho por hacer. 
        Puedes ayudarnos contándoles a tus amigos sobre nosotros y enviándonos 
        cualquier error o sugerencia de mejora. Si tienes alguna pregunta, ¡estamos 
        encantados de ayudarte!</p>
        
        <p>Un cordial saludo,<br>
        Robert</p>
        <p style='font-size: 12px; margin-top: 20px'>Correo electrónico: {1} | Teléfono: +49-178 186 68 48 (Robert)</p>
        ";
}
