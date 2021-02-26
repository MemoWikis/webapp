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
            TemplateJson loadbeforeLastLoadToken = null;
            TemplateJson loadAsLastToken = null;

            foreach (var obj in jsonObject)
            {
                var json = AddJsonTemplate(obj);

                if (obj.Load != null && obj.Load.Value.Equals("All"))
                {
                    var tempObj =
                        "[{\"TemplateName\":\"InlineText\",\"Content\":\"<h3 id=\\\"AllSubtopics\\\">Alle untergeordneten Themen</h3>\\n\"}]";
                    loadbeforeLastLoadToken = AddJsonTemplate(JsonConvert.DeserializeObject(tempObj), true);
                    loadAsLastToken = json;
                }
                else
                    tokens.Add(json);
            }
            if (loadAsLastToken != null)
                tokens.Add(loadbeforeLastLoadToken);
            tokens.Add(loadAsLastToken);

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
