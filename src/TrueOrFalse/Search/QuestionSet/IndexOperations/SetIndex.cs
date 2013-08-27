using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class SetIndex : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<SetSolrMap> _solrOperations;

        public SetIndex(ISolrOperations<SetSolrMap> solrOperations){
            _solrOperations = solrOperations;
        }

        public void Update(Set set)
        {
            _solrOperations.Add(ToSetSolrMap.Run(set));
            _solrOperations.Commit();
        }

        public void Delete(Set set)
        {
            _solrOperations.Delete(ToSetSolrMap.Run(set));
            _solrOperations.Commit();
        }
    }
}