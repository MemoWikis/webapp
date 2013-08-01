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
            var solrQuestionMap = new SetSolrMap
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

            solrOperations.Add(solrQuestionMap);
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

            Assert.That(Resolve<SearchSets>().Run("Set1", new Pager()).Count, Is.EqualTo(1)); ;
            Assert.That(Resolve<SearchSets>().Run("Set2 B", new Pager()).SetIds.Count, Is.EqualTo(1)); ;
        }
    }
}
