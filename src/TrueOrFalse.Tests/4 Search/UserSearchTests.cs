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
    public class UserSearchTests : BaseTest
    {
        [Test]
        public void Should_insert_and_query_document()
        {
            var solrMap = new UserSolrMap
            {
                Id = 1,
                DateCreated = DateTime.Now,
                Name = "John"
            };

            var solrOperations = Resolve<ISolrOperations<UserSolrMap>>();
            solrOperations.Delete(new SolrQuery("*:*"));
            solrOperations.Commit();

            solrOperations.Add(solrMap);
            solrOperations.Commit();

            var result = solrOperations.Query(new SolrQueryByField("Name", "John"));
            Assert.That(result.Count, Is.EqualTo(1));
        }

    }
}
