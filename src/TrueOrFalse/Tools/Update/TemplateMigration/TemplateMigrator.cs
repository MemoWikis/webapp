using System.Collections.Generic;
using SolrNet.Utils;
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
                    var content = "[\\&quot;";
                    var parts = MarkdownConverter.SplitMarkdown(markdown);

                    foreach (var part in parts)
                    {
                        if (part.IsCategoryNetwork)
                            continue;

                        if (part.IsTopicNavigation)
                        {
                            var clean = part.ToText().Replace("[[", "").Replace("]]", "");
                            var stringify = clean.Replace("\"", "\\&quot;");
                            content += "\n" + stringify + ",";
                        }
                        else
                        {
                            var html = MarkdownMarkdig.ToHtml(part.ToText()).Replace("\n", "<br>");
                            if (!IsNullOrEmpty(html))
                            {
                                var encodedHtml = html.Replace("\"", "\\\\&quot;");
                                content += "\n" + "{\\&quot;TemplateName\\&quot;:\\&quot;InlineText\\&quot;,\\&quot;Content\\&quot;:\\&quot;" + encodedHtml + "\\&quot; }" + ",";
                            }
                        }
                    }
                    if (content.EndsWith(","))
                        content = content.Remove(content.Length - 1);
                    content += "\\&quot;]";
                    if (content == "[\\&quot;\\&quot;]")
                        continue;
                    

                    content = HttpUtility.HtmlDecode(content);

                    category.Content = content;

                    Sl.CategoryRepo.UpdateBeforeEntityCacheInit(category);

                    Logg.r().Information("TemplateConversionMarkdownToContent: {categoryId} : {markdown} -- {content}", categoryId, markdown, content);
                }
            }
        }
    }
}