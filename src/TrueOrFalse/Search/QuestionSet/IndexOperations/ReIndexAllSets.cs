using SolrNet;

namespace TrueOrFalse.Search
{
    public class ReIndexAllSets : IRegisterAsInstancePerLifetime
    {
        private readonly SetRepo _setRepo;
        private readonly SetValuationRepo _setValuationRepo;
        private readonly ISolrOperations<SetSolrMap> _solrOperations;

        public ReIndexAllSets(
            SetRepo setRepo,
            SetValuationRepo setValuationRepo,
            ISolrOperations<SetSolrMap> solrOperations)
        {
            _setRepo = setRepo;
            _setValuationRepo = setValuationRepo;
            _solrOperations = solrOperations;
        }

        public void Run()
        {
            _solrOperations.Delete(new SolrQuery("*:*")); //Delete all sets
            _solrOperations.Commit();
            
            foreach (var set in _setRepo.GetAll())
                _solrOperations.Add(ToSetSolrMap.Run(set, _setValuationRepo.GetBy(set.Id)));

            _solrOperations.Commit();
        }
    }
}