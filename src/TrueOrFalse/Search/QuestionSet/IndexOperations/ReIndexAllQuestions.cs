using SolrNet;

namespace TrueOrFalse.Search
{
    public class ReIndexAllSets : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionSetRepository _setRepository;
        private readonly ISolrOperations<SetSolrMap> _solrOperations;

        public ReIndexAllSets(
            QuestionSetRepository setRepository, 
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