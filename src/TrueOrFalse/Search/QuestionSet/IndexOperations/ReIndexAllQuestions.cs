using SolrNet;

namespace TrueOrFalse.Search
{
    public class ReIndexAllSets : IRegisterAsInstancePerLifetime
    {
        private readonly SetRepository _setRepository;
        private readonly ISolrOperations<SetSolrMap> _solrOperations;

        public ReIndexAllSets(
            SetRepository setRepository, 
            ISolrOperations<SetSolrMap> solrOperations)
        {
            _setRepository = setRepository;
            _solrOperations = solrOperations;
        }

        public void Run()
        {
            _solrOperations.Delete(new SolrQuery("*:*")); //Delete all sets
            _solrOperations.Commit();
            
            foreach (var set in _setRepository.GetAll())
                _solrOperations.Add(ToSetSolrMap.Run(set));

            _solrOperations.Commit();
        }
    }
}