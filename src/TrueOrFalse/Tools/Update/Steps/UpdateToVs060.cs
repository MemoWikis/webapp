using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs060
    {
        public static void Run()
        {
            var session = Sl.Resolve<ISession>();

            var categories = session
                .QueryOver<Category>()
                .Where(c => c.Type == CategoryType.DailyIssue || c.Type == CategoryType.MagazineIssue || c.Type == CategoryType.WebsiteArticle)
                .List<Category>();

            foreach (var category in categories)
            {
                var anonymousType = new {Year = ""};
                anonymousType = JsonConvert.DeserializeAnonymousType(category.TypeJson, anonymousType);

                if (category.Type == CategoryType.DailyIssue)
                {
                    var typeModelDailyIssue = (CategoryTypeDailyIssue)category.GetTypeModel();
                    typeModelDailyIssue.PublicationDateYear = anonymousType.Year;
                    category.TypeJson = typeModelDailyIssue.ToJson();
                }

                if (category.Type == CategoryType.MagazineIssue)
                {
                    var typeModelMagazineIssue = (CategoryTypeMagazineIssue)category.GetTypeModel();
                    typeModelMagazineIssue.PublicationDateYear = anonymousType.Year;
                    category.TypeJson = typeModelMagazineIssue.ToJson();
                }

                if (category.Type == CategoryType.WebsiteArticle)
                {
                    var typeModelWebsiteArticle = (CategoryTypeWebsiteArticle)category.GetTypeModel();
                    typeModelWebsiteArticle.PublicationDateYear = anonymousType.Year;
                    category.TypeJson = typeModelWebsiteArticle.ToJson();
                }

                session.Update(category);
            }
        }
    }
}