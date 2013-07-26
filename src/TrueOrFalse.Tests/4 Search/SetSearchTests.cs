using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
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
    }
}
