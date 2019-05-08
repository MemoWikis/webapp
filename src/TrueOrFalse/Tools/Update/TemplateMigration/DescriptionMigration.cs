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
                var topicMarkdownBeforeUpdate = category.TopicMarkdown;
                var categoryId = category.Id;

                if (!IsNullOrWhiteSpace(category.Description) || !IsNullOrWhiteSpace(category.WikipediaURL) || !IsNullOrWhiteSpace(category.Url))
                {
                    string newMarkdown = "";

                    if (!IsNullOrWhiteSpace(category.Description))
                    {
                        newMarkdown = newMarkdown + category.Description + System.Environment.NewLine + System.Environment.NewLine;
                    }

                    if (!IsNullOrWhiteSpace(category.WikipediaURL))
                    {
                        newMarkdown = newMarkdown + category.WikipediaURL + System.Environment.NewLine + System.Environment.NewLine;
                        category.WikipediaURL = "";

                    }

                    if (!IsNullOrWhiteSpace(category.Url))
                    {
                        newMarkdown = newMarkdown + "[" + category.UrlLinkText + "](" + category.Url + ")" + System.Environment.NewLine + System.Environment.NewLine;
                        category.Url = "";
                    }

                    category.TopicMarkdown = newMarkdown + category.TopicMarkdown;
                }

                category.UrlLinkText = "";


                Sl.CategoryRepo.Update(category);

                Logg.r().Information("{categoryId} {beforeMarkdown} {afterMarkdown}", categoryId, topicMarkdownBeforeUpdate, category.TopicMarkdown);
            }
        }
    }
}