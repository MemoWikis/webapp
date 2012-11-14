using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SolrNet;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests
{
    public class QuestionSearchTests : BaseTest
    {
        [Test]
        public void Should_insert_and_query_document()
        {
            var solrQuestionMap = new QuestionSolrMap
                {
                    Id = 1,
                    DateCreated = DateTime.Now, 
                    CreatorId = 1,
                    Categories = {"Cat1", "Cat2"},
                    Title = "Title",
                    Description = "Description",
                    Solution = "Solution",
                    SolutionType = 2,
                    Quality = 70,
                    Views = 23424
                };
            solrQuestionMap.Categories.Add("Cat1");
            solrQuestionMap.Categories.Add("Cat2");

            var solr = Resolve<ISolrOperations<QuestionSolrMap>>();
            solr.Delete(new SolrQuery("*:*"));
            solr.Commit();

            solr.Add(solrQuestionMap);
            solr.Commit();

            var result = solr.Query(new SolrQueryByField("Solution", "Solution"));
            Assert.That(result.Count, Is.EqualTo(1));
        }
    }
}