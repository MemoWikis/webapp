public class PersonalPage
{
    public static Page GetPersonalPage(User user, PageRepository pageRepo)
    {
        return new Page
        {
            Name = user.Name + "s Wiki",
            Content = PersonalPageContent,
            Visibility = PageVisibility.Owner,
            Creator = user,
            Type = PageType.Standard,
            IsWiki = true
        };
    }

    public static string PersonalPageContent = "<h2>Herzlich willkommen,</h2>" +
                                                       "<p><strong>dies ist deine persönliche Startseite!</strong> Du kannst diesen Text leicht ändern, indem du einfach hier anfängst zu tippen. Probier mal!</p>" +
                                                       "<p>Wenn du Fragen hast, melde dich. Wir helfen dir gerne! :-)</p> " +
                                                       "<p><strong>Liebe Grüße, dein memoWikis-Team.</strong></p>";
}
