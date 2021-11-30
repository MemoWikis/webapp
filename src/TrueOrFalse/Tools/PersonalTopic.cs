public class PersonalTopic 
{
        public static void CreatePersonalCategory(User user)
        {
            Sl.CategoryRepo.Create(GetPersonalCategory(user));
        }

        public static Category GetPersonalCategory(User user)
        {
            return new Category
            {
                Name = user.Name + "s Wiki",
                Content = PersonalCategoryContent,
                Visibility = CategoryVisibility.Owner,
                Creator = user,
                Type = CategoryType.Standard,
                IsUserStartTopic = true
            };
        }

        public static string PersonalCategoryContent = "<h2>Herzlich willkommen,</h2>" +
                                                       "<p> <strong>dies ist deine persönliche Startseite!</strong> Du kannst diesen Text leicht ändern, indem du einfach hier anfängst zu tippen. Probier mal!</p>" +
                                                       "<p>Wenn du Fragen hast, melde dich. Wir helfen dir gerne! :-)</p> " +
                                                       "<p><b>Liebe Grüße, dein memucho-Team.</b></p>";
}
