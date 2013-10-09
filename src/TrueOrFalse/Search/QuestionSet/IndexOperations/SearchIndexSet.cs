using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class SearchIndexSet : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<SetSolrMap> _solrOperations;
        private  SetValuationRepository __setValuationRepo;

        private SetValuationRepository _setValuationRepo{
            get{
                if (__setValuationRepo == null)
                    __setValuationRepo = Sl.Resolve<SetValuationRepository>();

                return __setValuationRepo;
            }
        }

        public SearchIndexSet(ISolrOperations<SetSolrMap> solrOperations){
            _solrOperations = solrOperations;
        }

        public void Update(Set set)
        {
            _solrOperations.Add(ToSetSolrMap.Run(set, _setValuationRepo.GetBy(set.Id).Where(s => s.RelevancePersonal != -1)));
            _solrOperations.Commit();
        }

        public void Update(IEnumerable<Set> sets)
        {
            foreach(var set in sets)
                _solrOperations.Add(ToSetSolrMap.Run(set, _setValuationRepo.GetBy(set.Id).Where(s => s.RelevancePersonal != -1)));

            _solrOperations.Commit();
        }

        public void Delete(Set set)
        {
            _solrOperations.Delete(ToSetSolrMap.Run(set, Enumerable.Empty<SetValuation>()));
            _solrOperations.Commit();
        }

    }
}