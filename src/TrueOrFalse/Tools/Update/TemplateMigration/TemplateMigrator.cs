using System;
using System.Collections.Generic;

namespace TemplateMigration
{
    public partial class TemplateMigrator
    {
        public static void Start()
        {
            var allCategories = Sl.CategoryRepo.GetAll();

            foreach (var category in allCategories)
            {
                if (!String.IsNullOrEmpty(category.TopicMarkdown) && category.TopicMarkdown.Contains("DivStart"))
                {
                    category.TopicMarkdown = MarkdownConverter.ConvertMarkdown(category.TopicMarkdown);
                    Sl.CategoryRepo.UpdateBeforeEntityCacheInit(category);
                }
            }
        }
    }
}