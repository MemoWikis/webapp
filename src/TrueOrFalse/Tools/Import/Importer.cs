﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse
{
    public class Importer : IRegisterAsInstancePerLifetime
    {
        private readonly UserReadingRepo _userReadingRepo;
        private readonly CategoryRepository _categoryRepository;

        public Importer(UserReadingRepo userReadingRepo, CategoryRepository categoryRepository)
        {
            _userReadingRepo = userReadingRepo;
            _categoryRepository = categoryRepository;
        }

        public ImporterResult Run(string xml)
        {
            var document = XDocument.Parse(xml);

            var result = new ImporterResult();

            result.Categories = (from categoryElement in document.Root.Elements("category")
                                 select new CategoryCacheItem(categoryElement.Element("name").Value) { Id = Convert.ToInt32(categoryElement.Element("id").Value) });

            foreach (var category in result.Categories)
            {
                var categoryElement = document.Root.Elements("category").Single(x => x.Element("name").Value == category.Name);
                var parentCategories = (from relatedElementId in categoryElement.Element("relatedCategories").Elements("id")
                                              select result.Categories.Single(x => x.Id == Convert.ToInt32(relatedElementId.Value))).ToList();
                new ModifyRelationsForCategory(_categoryRepository).UpdateCategoryRelationsOfType(category.Id, parentCategories.Select(c => c.Id).ToList());
            }

            result.Questions = from questionElement in document.Root.Elements("question")
                               select new Question
                               {
                                   Text = questionElement.Element("text").Value,
                                   Description= questionElement.Element("description").Value,
                                   Visibility = (QuestionVisibility) Enum.Parse(typeof(QuestionVisibility), questionElement.Element("visibility").Value),
                                   Creator = _userReadingRepo.GetById(Convert.ToInt32(questionElement.Element("creatorId").Value)),
                                   Solution = questionElement.Element("solution").Value,
                                   Categories = _categoryRepository.GetByIds((from categoryIdElement in questionElement.Element("categories").Elements("id")
                                                 select result.Categories.SingleOrDefault(x => x.Id == Convert.ToInt32(categoryIdElement.Value))).Select(c => c.Id).ToList()) 
                               };

            foreach (var category in result.Categories)
            {
                category.Id = 0;
            }

            return result;
        }
    }
}
