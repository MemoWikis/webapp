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
                Content = "<h2>Herzlich willkommen,</h2>" +
                          "<p> <strong>dies ist deine persönliche Startseite!</strong> Du kannst diesen Text leicht ändern, indem du einfach hier anfängst zu tippen. Probier mal!</p>" +
                          "<p>Achtung: Dieses Thema ist (noch) öffentlich. Du kannst diese Seite rechts im Dreipunkt-Menü auf privat stellen. Dann ist dieses Thema nur für dich zu erreichen.</p> " +
                          "<p>Wenn du Fragen hast, melde dich. Wir helfen dir gerne! :-)</p> " +
                          "<p><b>Liebe Grüße, dein memucho-Team.</b></p>",
                Visibility = 0,
                Creator = user,
                Type = CategoryType.Standard,
                IsUserStartTopic = true
            };
        }
    }
