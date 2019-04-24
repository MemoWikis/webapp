using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace TemplateMigration
{
    public partial class DescriptionMigration
    {
        public static void Start()
        {
            var allCategories = Sl.CategoryRepo.GetAll();

            foreach (var category in allCategories)
            {
                if (!IsNullOrEmpty(category.Description) && !IsNullOrEmpty(category.WikipediaURL) && !IsNullOrEmpty(category.Url) && !IsNullOrEmpty(category.UrlLinkText))
                {
                    var topicMarkdownBeforeUpdate = category.TopicMarkdown;
                    var categoryId = category.Id;
                    string newMarkdown = "";

                    if (!IsNullOrEmpty(category.Description))
                    {
                        newMarkdown = newMarkdown + category.Description + System.Environment.NewLine;
                        category.Description = "";
                    }

                    if (!IsNullOrEmpty(category.WikipediaURL))
                    {
                        newMarkdown = newMarkdown + category.WikipediaURL + System.Environment.NewLine;
                        category.WikipediaURL = "";

                    }

                    if (!IsNullOrEmpty(category.Url))
                    {
                        newMarkdown = newMarkdown + "[" + category.UrlLinkText + "](" + category.Url + ")" + System.Environment.NewLine;
                        category.Url = "";
                    }

                    category.UrlLinkText = "";

                    newMarkdown = newMarkdown + category.TopicMarkdown;

                    category.TopicMarkdown = newMarkdown;

                    Sl.CategoryRepo.UpdateBeforeEntityCacheInit(category);

                    Logg.r().Information("{categoryId} {beforeMarkdown} {afterMarkdown}", categoryId, topicMarkdownBeforeUpdate, category.TopicMarkdown);
                }
            }
        }
    }
}