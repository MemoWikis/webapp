using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TrueOrFalse.Web;
using static System.String;

namespace TemplateMigration
{
    public class TopicNavigationMigration
    {
        public static void Run()
        {
            var allCategories = Sl.CategoryRepo.GetAll();
            foreach (var category in allCategories)
            {
                if (IsNullOrEmpty(category.Content))
                    continue;

                var categoryContentBeforeMigration = category.Content;
                var categoryId = category.Id;
                category.Content = RemoveTopicNavigation(category.Content, allCategories);

                Sl.CategoryRepo.UpdateBeforeEntityCacheInit(category);

                Logg.r().Information("MigrateContent: {categoryId} {beforeContentMigration} {afterContentMigration}", categoryId, categoryContentBeforeMigration, category.Content);
            }
        }
        public static string RemoveTopicNavigation(string categoryContent, IList<Category> allCategories)
        {
            var templates = GetTemplates(categoryContent).Where(t => t != null).ToList();

            var newContent = "";
            foreach (var template in templates)
            {
                if (template.TemplateName == "InlineText")
                {
                    var inlineTextToken = JsonConvert.DeserializeObject<InlineTextJson>(template.OriginalJson);
                    newContent += inlineTextToken.Content;
                }
                else if (template.TemplateName == "TopicNavigation")
                {
                    var topicNavigationToken = JsonConvert.DeserializeObject<TopicNavigationJson>(template.OriginalJson);
                    if (topicNavigationToken.Load == null || topicNavigationToken.Load == "All")
                        continue;

                    var newCategoryListHtml = "<p><ul>";
                    var categoryIds = topicNavigationToken.Load.Split(',').ToList().ConvertAll(int.Parse);
                    foreach (var categoryId in categoryIds)
                    {
                        var category = allCategories.FirstOrDefault(c => c.Id == categoryId);
                        var url = "/" + UriSanitizer.Run(category.Name) + "/" + category.Id;
                        var html = "\n<li><a href=\"" + url + "\">" + category.Name + "</a></li>";
                        newCategoryListHtml += html;
                    }

                    newCategoryListHtml += "</ul></p>";
                    newContent += newCategoryListHtml;
                }

            }

            return newContent;
        }

        public static List<TemplateJson> GetTemplates(string contentString)
        {
            dynamic jsonObject = JsonConvert.DeserializeObject(contentString);

            var tokens = new List<TemplateJson>();

            foreach (var obj in jsonObject)
            {
                var json = AddJsonTemplate(obj);
                if (obj.TemplateName.Value.Equals("InlineText"))
                    tokens.Add(json);
                else if (obj.Load == null || obj.Load.Value.Equals("All"))
                    continue;
                else if (obj.Load != null || !obj.Load.Value.Equals("All"))
                    tokens.Add(json);
            }

            return tokens;
        }

        public static TemplateJson AddJsonTemplate(dynamic obj, bool isBeforeLast = false)
        {
            var json = new TemplateJson
            {
                TemplateName = isBeforeLast ? obj.First.TemplateName : obj.TemplateName
            };
            json.OriginalJson = isBeforeLast ? obj.First.ToString() : obj.ToString();

            return json;
        }
        public class TemplateJson
        {
            public string TemplateName;
            public string OriginalJson = "";

            [JsonIgnore]
            public string InlineText = "";
        }

        public class TopicNavigationJson
        {
            public string Title;
            public string Text;
            public string Load;
            public string Order;
        }

        public class InlineTextJson
        {
            public string TemplateName;
            public string Content;
        }
    }
}
