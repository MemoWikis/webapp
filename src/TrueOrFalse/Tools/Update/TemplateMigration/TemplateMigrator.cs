using System.Collections.Generic;
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
                    var content = "";
                    if (!category.TopicMarkdown.Contains("[[") && !category.TopicMarkdown.Contains("]]"))
                    {
                        var html = MarkdownMarkdig.ToHtml(category.TopicMarkdown).Replace("\n", "<br>");
                        content = "{ \" TemplateName\":\"InlineText\", \"Content\": \"" + html + "\" }";
                    }

                    category.Content = content;

                    category.TopicMarkdown = MarkdownConverter.ConvertTemplates(category.TopicMarkdown);

                    Sl.CategoryRepo.UpdateBeforeEntityCacheInit(category);

                    Logg.r().Information("TemplateConversionMarkdownToContent: {categoryId} : {markdown} -- {content}", categoryId, markdown, content);
                }
            }
        }
    }
}