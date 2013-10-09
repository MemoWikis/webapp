using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Seedworks.Lib.Persistence;
using SolrNet;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests
{
    public class SetSearchTests : BaseTest
    {
        [Test]
        public void Should_insert_and_query_document()
        {
            var solrSetMap = new SetSolrMap
            {
                Id = 1,
                DateCreated = DateTime.Now,
                CreatorId = 1,
                Name = "Name",
                Text = "Text",
                AllQuestionsBodies = "Foo bar",
                AllQuestionsTitles = "Lorem ipsum",
            };

            var solrOperations = Resolve<ISolrOperations<SetSolrMap>>();
            solrOperations.Delete(new SolrQuery("*:*"));
            solrOperations.Commit();

            solrOperations.Add(solrSetMap);
            solrOperations.Commit();

            var result = solrOperations.Query(new SolrQueryByField("FullTextExact", "Foo"));
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void Should_search_sets()
        {
            ContextSet.New()
                .AddSet("Set1 A")
                .AddSet("Set2 B")
                .Persist();

            Resolve<ReIndexAllSets>().Run();

            Assert.That(Resolve<SearchSets>().Run("Set").Count, Is.EqualTo(2)); ;
            Assert.That(Resolve<SearchSets>().Run("\"Set2 B\"").SetIds.Count, Is.EqualTo(1)); ;
        }

        [Test]
        public void Should_search_sets_and_filter_by_creator()
        {
            var userContext = ContextUser.New().Add("user 1").Add("user 2").Persist();
            var user1 = userContext.All.First();

            ContextSet.New()
                .AddSet("Set1 A", creator: user1)
                .AddSet("Set2 B", creator: user1)
                .AddSet("Set3 B")
                .Persist();
            
            Assert.That(Resolve<SearchSets>().Run("Set", user1.Id).Count, Is.EqualTo(2)); ;
        }

        [Test]
        public void Should_search_sets_part_of_active_knowledge()
        {
            var userContext = ContextUser.New().Add("user 1").Add("user 2").Persist();
            var user1 = userContext.All.First();

            var context = ContextSet.New()
                .AddSet("Set1 A", creator: user1)
                .AddSet("Set2 B", creator: user1)
                .AddSet("Set3 B")
                .Persist();

            var setValuation = new SetValuation { SetId = context.All[1].Id, UserId = user1.Id, RelevancePersonal = 7 };
            Resolve<SetValuationRepository>().Create(new List<SetValuation> { setValuation });

            Assert.That(Resolve<SearchSets>().Run("Set", valuatorId: user1.Id).Count, Is.EqualTo(1));

            When_a_setValuation_is_removed_it_should_be_removed_from_search_result(setValuation, context.All[1], user1);
        }

        public void When_a_setValuation_is_removed_it_should_be_removed_from_search_result(
            SetValuation setValuationToRemove, Set setWithRemovedValuation, User user)
        {
            setValuationToRemove.RelevancePersonal = -1;
            Resolve<SetValuationRepository>().Update(setValuationToRemove);
            Resolve<SearchIndexSet>().Update(setWithRemovedValuation);

            Assert.That(Resolve<SearchSets>().Run("Set", valuatorId: user.Id).Count, Is.EqualTo(0));
        }
    }
}
