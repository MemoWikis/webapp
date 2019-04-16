﻿using System.Collections.Generic;

namespace TemplateMigration
{
    public partial class TemplateMigrator
    {
        public static void Start()
        {
            var allCategories = Sl.CategoryRepo.GetAll();

            foreach (var category in allCategories)
                category.TopicMarkdown = MarkdownConverter.ConvertMarkdown(category.TopicMarkdown);
        }

    }
}