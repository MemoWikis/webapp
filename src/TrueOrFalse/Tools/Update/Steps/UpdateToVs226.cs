using System;
using System.Linq;
using NHibernate;
using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs226
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `user`
                    ADD COLUMN `StartTopicId` Int DEFAULT 0"
                ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `Category`
                    ADD COLUMN `IsUserStartTopic` BIT DEFAULT 0"
                ).ExecuteUpdate();

            var users =   Sl.UserRepo.GetAll();
          
          foreach (var user in users)
          {
              var category = new Category
              {
                  Name = user.Name + "s Wiki",
                  Content = "<h2>Herzlich willkommen, dies ist dein persönliches Wiki!</h2>" +
                            "<p> Du kannst diesen Text leicht ändern, in dem du einfach hier anfängt zu tippen. Probier mal!</p>" +
                            " <p> Achtung: Dieses Thema ist(noch) öffentlich. Du kannst diese Seite im 3 - Punkte - Menü rechts auf privat stellen.</p> " +
                            "<p>Dann ist dieses Thema nur für dich zu erreichen. Wir helfen die gerne! Wenn du Fragen hast, melde dich. :-)</p> " +
                            "<p><b>Liebe Grüße, dein memucho - Team.</b></p>",
                  Visibility = 0,
                  Creator = user,
                  Type = CategoryType.Standard,
                  IsUserStartTopic = true
              }; 
              Sl.CategoryRepo.CreateOnlyDb(category);
              user.StartTopicId = category.Id; 
          }
        }
    }
}