﻿using System;
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
        public IEnumerable<Question> Questions { get; private set; }
        public IEnumerable<Category> Categories { get; private set; }

        public Importer(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Run(string xml)
        {
            var document = XDocument.Parse(xml);

            Questions = from questionElement in document.Root.Elements("question")
                        select new Question
                        {
                            Text = questionElement.Element("text").Value,
                            Description= questionElement.Element("description").Value,
                            Visibility = (QuestionVisibility) Enum.Parse(typeof(QuestionVisibility), questionElement.Element("visibility").Value),
                            Creator = _userRepository.GetById(Convert.ToInt32(questionElement.Element("creatorId").Value)),
                            Solution = questionElement.Element("solution").Value
                        };

            Categories = from categoryElement in document.Root.Elements("category")
                         select new Category
                         {
                             Name = categoryElement.Element("name").Value,
                             SubCategories = (from subCategoryElement in categoryElement.Elements("subCategory")
                                              select new SubCategory
                                              {
                                                  Name = subCategoryElement.Element("name").Value,
                                                  Type = (SubCategoryType) Enum.Parse(typeof(SubCategoryType), subCategoryElement.Element("type").Value),
                                                  Items = (from itemElement in subCategoryElement.Elements("item")
                                                           select new SubCategoryItem
                                                                      {
                                                                          Name = itemElement.Element("name").Value
                                                                      }).ToList()
                                              }).ToList()
                         };

        }
    }
}
