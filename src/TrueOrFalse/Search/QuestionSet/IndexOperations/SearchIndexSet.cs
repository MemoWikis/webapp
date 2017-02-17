using System.Collections.Generic;
using System.Linq;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class SearchIndexSet : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<SetSolrMap> _solrOperations;
        private  SetValuationRepo __setValuationRepo;

        private SetValuationRepo _setValuationRepo{
            get{
                if (__setValuationRepo == null)
                    __setValuationRepo = Sl.Resolve<SetValuationRepo>();

                return __setValuationRepo;
            }
        }

        public SearchIndexSet(ISolrOperations<SetSolrMap> solrOperations){
            _solrOperations = solrOperations;
        }

        public void Update(Set set)
        {
            _solrOperations.Add(ToSetSolrMap.Run(set, _setValuationRepo.GetBy(set.Id)));
            _solrOperations.Commit();
        }

        public void Update(IEnumerable<Set> sets)
        {
            foreach(var set in sets)
                _solrOperations.Add(ToSetSolrMap.Run(set, _setValuationRepo.GetBy(set.Id)));

            _solrOperations.Commit();
        }

        public void Delete(Set set)
        {
            _solrOperations.Delete(ToSetSolrMap.Run(set, Enumerable.Empty<SetValuation>()));
            _solrOperations.Commit();
        }

    }
}