using System.Security.Cryptography.X509Certificates;

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
                Content = "<h2>Herzlich willkommen, dies ist deine persönliche Startseite!</h2>" +
                          "<p> Du kannst diesen Text leicht ändern, indem du einfach hier anfängst zu tippen. Probier mal!</p>" +
                          " <p> Achtung: Dieses Thema ist (noch) öffentlich. Du kannst diese Seite rechts im Dreipunkt Menü privat stellen.</p> " +
                          "<p>Dann ist dieses Thema nur für dich zu erreichen. Wir helfen dir gerne! Wenn du Fragen hast, melde dich. :-)</p> " +
                          "<p><b>Liebe Grüße, dein memucho-Team.</b></p>",
                Visibility = 0,
                Creator = user,
                Type = CategoryType.Standard,
                IsUserStartTopic = true
            };
        }
    }
