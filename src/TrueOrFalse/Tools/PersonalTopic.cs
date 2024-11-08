public class PersonalTopic 
{
    public static Page GetPersonalCategory(User user, PageRepository pageRepo)
    {
        return new Page
        {
            Name = user.Name + "s Wiki",
            Content = PersonalCategoryContent,
            Visibility = PageVisibility.Owner,
            Creator = user,
            Type = PageType.Standard,
            IsUserStartTopic = true
        };
    }

    public static string PersonalCategoryContent = "<h2>Herzlich willkommen,</h2>" +
                                                       "<p><strong>dies ist deine persönliche Startseite!</strong> Du kannst diesen Text leicht ändern, indem du einfach hier anfängst zu tippen. Probier mal!</p>" +
                                                       "<p>Wenn du Fragen hast, melde dich. Wir helfen dir gerne! :-)</p> " +
                                                       "<p><strong>Liebe Grüße, dein memucho-Team.</strong></p>";
}
