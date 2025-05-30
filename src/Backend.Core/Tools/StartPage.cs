﻿public static class StartPage
{
    public static Page Get(User user)
    {
        return new Page
        {
            Name = user.Name + "s Wiki",
            Content = GetContentByUiLanguage(user.UiLanguage),
            Visibility = PageVisibility.Private,
            Creator = user,
            IsWiki = true
        };
    }

    public static string ContentDe =
        "<h2>Herzlich willkommen,</h2>" +
        "<p><strong>dies ist deine persönliche Startseite!</strong> Du kannst diesen Text leicht ändern, indem du einfach hier anfängst zu tippen. Probier mal!</p>" +
        "<p>Wenn du Fragen hast, melde dich. Wir helfen dir gerne! :-)</p> " +
        "<p><strong>Liebe Grüße, dein memoWikis-Team.</strong></p>";

    public static string ContentEn =
        "<h2>Welcome,</h2>" +
        "<p><strong>this is your personal homepage!</strong> You can easily change this text by simply starting to type here. Give it a try!</p>" +
        "<p>If you have any questions, just let us know. We’re happy to help! :-)</p> " +
        "<p><strong>Best regards, your memoWikis team.</strong></p>";

    public static string ContentFr =
        "<h2>Bienvenue,</h2>" +
        "<p><strong>c'est votre page d'accueil personnelle&nbsp;!</strong> Vous pouvez facilement modifier ce texte en commençant simplement à taper ici. Essayez&nbsp;!</p>" +
        "<p>Si vous avez des questions, faites-le nous savoir. Nous sommes là pour vous aider&nbsp;! :-)</p> " +
        "<p><strong>Cordialement, votre équipe memoWikis.</strong></p>";

    public static string ContentEs =
        "<h2>Bienvenido,</h2>" +
        "<p><strong>esta es tu página de inicio personal.</strong> Puedes cambiar este texto fácilmente simplemente comenzando a escribir aquí. ¡Pruébalo!</p>" +
        "<p>Si tienes alguna pregunta, háznoslo saber. ¡Estamos encantados de ayudar! :-)</p> " +
        "<p><strong>Saludos cordiales, tu equipo de memoWikis.</strong></p>";


    private static string GetContentByUiLanguage(string languageCode)
    {
        var language = LanguageExtensions.GetLanguage(languageCode);

        switch (language)
        {
            case Language.German:
                return ContentDe;
            case Language.English:
                return ContentEn;
            case Language.French:
                return ContentFr;
            case Language.Spanish:
                return ContentEs;
            default: return ContentEn;
        }
    }
}
