using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TrueOrFalse.Core
{
    public class Importer
    {
        public IEnumerable<Question> Questions { get; private set; }
        public IEnumerable<Category> Categories { get; private set; }

        public Importer(string xml)
        {
            Read(xml);
        }

        private void Read(string xml)
        {
            var document = XDocument.Parse(xml);

            Questions = from questionElement in document.Root.Elements("question")
                        select new Question
                        {
                            Text = questionElement.Element("text").Value,
                            Answers = (from answerElement in questionElement.Elements("answer")
                                       select new Answer
                                       {
                                           Text = answerElement.Element("text").Value
                                       }).ToList()
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
