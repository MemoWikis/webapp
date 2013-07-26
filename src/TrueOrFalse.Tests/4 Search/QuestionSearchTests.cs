using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Seedworks.Lib.Persistence;
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

            var solrOperations = Resolve<ISolrOperations<QuestionSolrMap>>();
            solrOperations.Delete(new SolrQuery("*:*"));
            solrOperations.Commit();

            solrOperations.Add(solrQuestionMap);
            solrOperations.Commit();

            var result = solrOperations.Query(new SolrQueryByField("FullTextExact", "Solution"));
            Assert.That(result.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void Should_reindex_all_questions()
        {
            ContextQuestion.New()
                .AddQuestion("Question1", "Answer2").AddCategory("A").Persist()
                .AddQuestion("Question2", "Answer3").AddCategory("B").Persist()
                .AddQuestion("Juliane", "Answer3").AddCategory("B").Persist();

            Resolve<ReIndexAllQuestions>().Run();

            Assert.That(Resolve<SearchQuestions>().Run("Juliane", new Pager()).Count, Is.EqualTo(1)); ;
            Assert.That(Resolve<SearchQuestions>().Run("Juliane", new Pager()).QuestionIds.Count, Is.EqualTo(1)); ;
            Assert.That(Resolve<SearchQuestions>().Run("Question2", new Pager()).Count, Is.EqualTo(1)); ;
        }

        [Test]
        public void Should_get_paged_result()
        {
            var context = ContextQuestion.New();
            Enumerable.Range(1, 50).ToList().ForEach(x => context.AddQuestion("Question" + x, "Answer" + x).AddCategory("Question"));
            context.Persist();

            Resolve<ReIndexAllQuestions>().Run();

            Assert.That(Resolve<SearchQuestions>().Run("Question35", new Pager { PageSize = 10 }).Count, Is.EqualTo(1));//Title
            Assert.That(Resolve<SearchQuestions>().Run("Answer35", new Pager { PageSize = 10 }).Count, Is.EqualTo(1));//Title

            var result = Resolve<SearchQuestions>().Run("Question", new Pager { PageSize = 10 });
            Assert.That(result.Count, Is.EqualTo(50));//Category
            Assert.That(result.QuestionIds.Count, Is.EqualTo(10));//Result is always paged
        }
    }
}