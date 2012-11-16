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
                    Text = "Title",
                    Description = "Description",
                    Solution = "Solution",
                    SolutionType = 2,
                    AvgQuality = 70,
                    Views = 23424
                };
            solrQuestionMap.Categories.Add("Cat1");
            solrQuestionMap.Categories.Add("Cat2");

            var solrOperations = Resolve<ISolrOperations<QuestionSolrMap>>();
            solrOperations.Delete(new SolrQuery("*:*"));
            solrOperations.Commit();

            solrOperations.Add(solrQuestionMap);
            solrOperations.Commit();

            var result = solrOperations.Query(new SolrQueryByField("Solution", "Solution"));
            Assert.That(result.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void Should_reindex_all_questions()
        {
            ContextQuestion.New()
                .AddQuestion("Question1", "Answer2").AddCategory("A").Persist()
                .AddQuestion("Question2", "Answer3").AddCategory("B").Persist()
                .AddQuestion("Question3", "Answer3").AddCategory("B").Persist();

            Resolve<ReIndexAllQuestions>().Run();

            var solrOperations = Resolve<ISolrOperations<QuestionSolrMap>>();
            var result = solrOperations.Query("FullText:Question1");
            Assert.That(result.Count, Is.EqualTo(1));


        }
    }
}