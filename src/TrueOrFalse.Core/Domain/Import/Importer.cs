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
        }
    }
}
