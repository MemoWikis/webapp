using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor.Tokenizer;
using Newtonsoft.Json;

namespace TemplateMigration
{
    public class TopicNavigationMigration
    {
        public static string RemoveTopicNavigation(string categoryContent)
        {
            var tokens = GetTokens(categoryContent);

            var newString = "";
            var newContent = "";
            tokens = tokens.Where(t => t != null).ToList();
            foreach (TemplateJson token in tokens)
            {
                if (token.TemplateName == "InlineText")
                {
                    var inlineTextToken = JsonConvert.DeserializeObject<InlineTextJson>(token.OriginalJson);
                    newContent += inlineTextToken.Content;
                }
                else if (token.TemplateName == "TopicNavigation")
                {
                    var topicNavigationToken = JsonConvert.DeserializeObject<TopicNavigationJson>(token.OriginalJson);
                    if (topicNavigationToken.Load == null || topicNavigationToken.Load == "All")
                        continue;
                    else
                    {
                        var categoryIdList = topicNavigationToken.Load.Split(',').ToList().ConvertAll(int.Parse);

                    }
                }

            }

            return newContent;
        }

        public static List<TemplateJson> GetTokens(string contentString)
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
