using SolrNet;

namespace TrueOrFalse.Search
{
    public class ReIndexAllSets : IRegisterAsInstancePerLifetime
    {
        private readonly SetRepository _setRepository;
        private readonly SetValuationRepository _setValuationRepository;
        private readonly ISolrOperations<SetSolrMap> _solrOperations;

        public ReIndexAllSets(
            SetRepository setRepository,
            SetValuationRepository setValuationRepository,
            ISolrOperations<SetSolrMap> solrOperations)
        {
            _setRepository = setRepository;
            _setValuationRepository = setValuationRepository;
            _solrOperations = solrOperations;
        }

        public void Run()
        {
            _solrOperations.Delete(new SolrQuery("*:*")); //Delete all sets
            _solrOperations.Commit();
            
            foreach (var set in _setRepository.GetAll())
                _solrOperations.Add(ToSetSolrMap.Run(set, _setValuationRepository.GetBy(set.Id)));

            _solrOperations.Commit();
        }
    }
}