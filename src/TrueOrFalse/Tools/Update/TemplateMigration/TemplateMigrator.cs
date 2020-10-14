using System.Collections.Generic;
using Newtonsoft.Json;
using TrueOrFalse.Web;
using static System.String;

namespace TemplateMigration
{
    public class TemplateMigrator
    {
        public static void Start()
        {
            var allCategories = Sl.CategoryRepo.GetAll();

            foreach (var category in allCategories)
            {
                if (!IsNullOrEmpty(category.TopicMarkdown) && category.TopicMarkdown.Contains("DivStart"))
                {
                    var topicMarkdownBeforeUpdate = category.TopicMarkdown;
                    var categoryId = category.Id;
                    category.TopicMarkdown = MarkdownConverter.ConvertMarkdown(category.TopicMarkdown);

                    Sl.CategoryRepo.UpdateBeforeEntityCacheInit(category);

                    Logg.r().Information("{categoryId} {beforeMarkdown} {afterMarkdown}", categoryId, topicMarkdownBeforeUpdate, category.TopicMarkdown);
                }
            }
        }

        public static void MigrateContentModules()
        {
            var allCategories = Sl.CategoryRepo.GetAll();

            foreach (var category in allCategories)
            {
                if (!IsNullOrEmpty(category.TopicMarkdown))
                {
                    if (!category.TopicMarkdown.Contains("[["))
                        continue;

                    var topicMarkdownBeforeUpdate = category.TopicMarkdown;
                    var categoryId = category.Id;
                    category.TopicMarkdown = MarkdownConverter.ConvertTemplates(category.TopicMarkdown);

                    Sl.CategoryRepo.UpdateBeforeEntityCacheInit(category);

                    Logg.r().Information("ContentModuleMigration: {categoryId} : {beforeMarkdown} -- {afterMarkdown}", categoryId, topicMarkdownBeforeUpdate, category.TopicMarkdown);
                }
            }
        }

        public static void MigrateTopicMarkdownToContent()
        {
            var allCategories = Sl.CategoryRepo.GetAll();

            foreach (var category in allCategories)
            {
                if (!IsNullOrEmpty(category.TopicMarkdown))
                {

                    var categoryId = category.Id;
                    var markdown = category.TopicMarkdown;
                    var contentList = new List<dynamic>();
                    var parts = MarkdownConverter.SplitMarkdown(markdown);

                    foreach (var part in parts)
                    {
                        if (part.IsCategoryNetwork)
                            continue;

                        if (part.IsTopicNavigation)
                        {
                            var clean = part
                                .ToText()
                                .Replace("[[", "")
                                .Replace("]]", "")
                                .Replace("&quot;", @"""");
                            var json = JsonConvert.DeserializeObject(clean);

                            contentList.Add(json);
                        }
                        else
                        {
                            var html = MarkdownMarkdig.ToHtml(part.ToText()).Replace("\n", "<br>");
                            if (!IsNullOrEmpty(html))
                            {
                                var inlineTextJson = new InlineTextJson
                                {
                                    TemplateName = "InlineText",
                                    Content = html
                                };
                                contentList.Add(inlineTextJson);
                            }
                        }
                    }

                    if (contentList.Count < 1)
                        continue;

                    var content = JsonConvert.SerializeObject(contentList);
                    category.Content = content;

                    Sl.CategoryRepo.UpdateBeforeEntityCacheInit(category);

                    Logg.r().Information("TemplateConversionMarkdownToContent: {categoryId} : {markdown} -- {content}", categoryId, markdown, content);
                }
            }
        }

        private class TopicNavigationJson
        {
            public string Title;
            public string Text;
            public string Load;
            public string Order;
        }

        private class InlineTextJson
        {
            public string TemplateName;
            public string Content;
        }


    }
}