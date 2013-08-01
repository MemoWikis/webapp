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
    public class CategorySearchTests : BaseTest
    {
        [Test]
        public void Should_insert_and_query_document()
        {
            var categorySolrMap = new CategorySolrMap
            {
                Id = 1,
                DateCreated = DateTime.Now,
                CreatorId = 1,
                Name = "Name",
                Description = "Foo Bar"
            };

            var solrOperations = Resolve<ISolrOperations<CategorySolrMap>>();
            solrOperations.Delete(new SolrQuery("*:*"));
            solrOperations.Commit();

            solrOperations.Add(categorySolrMap);
            solrOperations.Commit();

            var result = solrOperations.Query(new SolrQueryByField("FullTextExact", "Foo"));
            Assert.That(result.Count, Is.EqualTo(1));
        }
    }
}
