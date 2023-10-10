public class PersonalTopic 
{
    public static Category GetPersonalCategory(User user)
    {
        var wikiName = user.Name + "s Wiki";
        var counter = 1;
        while (Sl.CategoryRepo.Exists(wikiName))
        {
            wikiName = user.Name + "s Wiki (" + counter + ")";
            counter++;
        }
            
        return new Category
        {
            Name = wikiName,
            Content = PersonalCategoryContent,
            Visibility = CategoryVisibility.Owner,
            Creator = user,
            Type = CategoryType.Standard,
            IsUserStartTopic = true
        };
    }

    public static string PersonalCategoryContent = "<h2>Herzlich willkommen,</h2>" +
                                                       "<p><strong>dies ist deine persönliche Startseite!</strong> Du kannst diesen Text leicht ändern, indem du einfach hier anfängst zu tippen. Probier mal!</p>" +
                                                       "<p>Wenn du Fragen hast, melde dich. Wir helfen dir gerne! :-)</p> " +
                                                       "<p><strong>Liebe Grüße, dein memucho-Team.</strong></p>";
}
