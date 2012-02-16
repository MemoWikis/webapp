using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core
{
    public class Importer : IRegisterAsInstancePerLifetime
    {
        private readonly UserRepository _userRepository;

        public Importer(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ImporterResult Run(string xml)
        {
            var document = XDocument.Parse(xml);

            var result = new ImporterResult();

            result.Categories = (from categoryElement in document.Root.Elements("category")
                                 select new Category(categoryElement.Element("name").Value) { Id = Convert.ToInt32(categoryElement.Element("id").Value) }).ToList();

            foreach (var category in result.Categories)
            {
                var categoryElement = document.Root.Elements("category").Single(x => x.Element("name").Value == category.Name);
                category.RelatedCategories = (from relatedElementId in categoryElement.Element("relatedCategories").Elements("id")
                                              select result.Categories.Single(x => x.Id == Convert.ToInt32(relatedElementId.Value))).ToList();
            }

            result.Questions = from questionElement in document.Root.Elements("question")
                               select new Question
                               {
                                   Text = questionElement.Element("text").Value,
                                   Description= questionElement.Element("description").Value,
                                   Visibility = (QuestionVisibility) Enum.Parse(typeof(QuestionVisibility), questionElement.Element("visibility").Value),
                                   Creator = _userRepository.GetById(Convert.ToInt32(questionElement.Element("creatorId").Value)),
                                   Solution = questionElement.Element("solution").Value,
                                   Categories = (from categoryIdElement in questionElement.Element("categories").Elements("id")
                                                 select result.Categories.Single(x => x.Id == Convert.ToInt32(categoryIdElement.Value))).ToList() 
                               };

            foreach (var category in result.Categories)
            {
                category.Id = 0;
            }

            return result;
        }
    }
}
