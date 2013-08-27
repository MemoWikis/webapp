using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class SearchIndexUser : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<UserSolrMap> _solrOperations;

        public SearchIndexUser(ISolrOperations<UserSolrMap> solrOperations){
            _solrOperations = solrOperations;
        }

        public void Update(User user)
        {
            _solrOperations.Add(ToUserSolrMap.Run(user));
            _solrOperations.Commit();
        }

        public void Delete(User user)
        {
            _solrOperations.Delete(ToUserSolrMap.Run(user));
            _solrOperations.Commit();
        }
    }
}