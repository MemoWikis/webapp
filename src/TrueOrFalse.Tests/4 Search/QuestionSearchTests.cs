using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NHibernate;
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
            Resolve<ISession>().Delete("FROM QuestionValuation");

            var context = ContextQuestion.New()
                .AddQuestion("Question1", "Answer2").AddCategory("A").Persist()
                .AddQuestion("Question2", "Answer3").AddCategory("B").Persist()
                .AddQuestion("Juliane Misdom", "Answer3").AddCategory("B").Persist();

            Resolve<ReIndexAllQuestions>().Run();

            Assert.That(Resolve<SearchQuestions>().Run("Juliane", new Pager()).Count, Is.EqualTo(1));
            Assert.That(Resolve<SearchQuestions>().Run("Juliane Misdom", new Pager()).QuestionIds.Count, Is.EqualTo(1)); ;
            Assert.That(Resolve<SearchQuestions>().Run("Question2", new Pager()).Count, Is.EqualTo(2)); ;

            Should_filter_by_creator_id(context);
        }

        public void Should_filter_by_creator_id(ContextQuestion contextQuestion)
        {
            Assert.That(Resolve<SearchQuestions>().Run(
                "", new Pager(), creatorId: contextQuestion.Creator.Id).Count, Is.EqualTo(3));
        }


        [Test]
        public void Should_filter_by_valuator_id()
        {
            Resolve<ISession>().Delete("FROM QuestionValuation");
            Resolve<ISession>().Delete("FROM Question");

            var context = ContextQuestion.New()
                .AddQuestion("Question1", "Answer2").AddCategory("A")
                .AddQuestion("Question2", "Answer3").AddCategory("B")
                .AddQuestion("Juliane Misdom", "Answer3").AddCategory("B")
                .Persist();

            Resolve<QuestionValuationRepository>().Create(new List<QuestionValuation>{
                new QuestionValuation { RelevancePersonal = 70, QuestionId = context.All[0].Id, UserId = 2 },
                new QuestionValuation { RelevancePersonal = 15, QuestionId = context.All[1].Id, UserId = 2 }
            });

            Assert.That(Resolve<SearchQuestions>().Run("Juliane", new Pager(), valuatorId:2).Count, Is.EqualTo(0));
            Assert.That(Resolve<SearchQuestions>().Run("", new Pager(), valuatorId: 2).Count, Is.EqualTo(2));
        }

        [Test]
        public void Should_get_paged_result()
        {
            var context = ContextQuestion.New();
            Enumerable.Range(1, 50).ToList().ForEach(x => context.AddQuestion("Question" + x, "Answer" + x).AddCategory("Question"));
            context.Persist();

            Resolve<ReIndexAllQuestions>().Run();

            Assert.That(Resolve<SearchQuestions>().Run("\"Question35\"", new Pager { PageSize = 10 }).Count, Is.EqualTo(1));//Title
            Assert.That(Resolve<SearchQuestions>().Run("\"Answer35\"", new Pager { PageSize = 10 }).Count, Is.EqualTo(1));//Title

            var result = Resolve<SearchQuestions>().Run("Question", new Pager { PageSize = 10 });
            Assert.That(result.Count, Is.EqualTo(50));//Category
            Assert.That(result.QuestionIds.Count, Is.EqualTo(10));//Result is always paged
        }

        [Test]
        public void Should_order_search_result()
        {
            var context = ContextQuestion.New()
                .AddQuestion("Question1", "Answer1").TotalQualityAvg(10).TotalValuationAvg(50)
                .AddQuestion("Question2", "Answer2").TotalQualityAvg(50).TotalValuationAvg(1)
                .AddQuestion("Question3", "Answer3").TotalValuationAvg(99)
                .Persist();

            var result = Resolve<SearchQuestions>().Run("Question", 
                new Pager { PageSize = 10 }, orderBy: SearchQuestionsOrderBy.Quality);
            var questions = Resolve<QuestionRepository>().GetByIds(result.QuestionIds);

            Assert.That(questions.Count, Is.EqualTo(3));
            Assert.That(questions[0].Text, Is.EqualTo("Question2"));
            Assert.That(questions[1].Text, Is.EqualTo("Question1"));
            Assert.That(questions[2].Text, Is.EqualTo("Question3"));

            result = Resolve<SearchQuestions>().Run("Question",
                new Pager { PageSize = 10 }, orderBy: SearchQuestionsOrderBy.Valuation);
            questions = Resolve<QuestionRepository>().GetByIds(result.QuestionIds);
            
            Assert.That(questions[0].Text, Is.EqualTo("Question3"));
            Assert.That(questions[1].Text, Is.EqualTo("Question1"));
            Assert.That(questions[2].Text, Is.EqualTo("Question2"));
        }
    }
}