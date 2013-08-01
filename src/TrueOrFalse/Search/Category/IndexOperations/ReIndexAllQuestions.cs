using SolrNet;

namespace TrueOrFalse.Search
{
    public class ReIndexAllCategories : IRegisterAsInstancePerLifetime
    {
        private readonly CategoryRepository _categoryRepo;
        private readonly ISolrOperations<CategorySolrMap> _solrOperations;

        public ReIndexAllCategories(
            CategoryRepository categoryRepo,
            ISolrOperations<CategorySolrMap> solrOperations)
        {
            _categoryRepo = categoryRepo;
            _solrOperations = solrOperations;
        }

        public void Run()
        {
            _solrOperations.Delete(new SolrQuery("*:*")); //Delete all sets
            _solrOperations.Commit();
            
            foreach (var category in _categoryRepo.GetAll())
                _solrOperations.Add(ToCategorytSolrMap.Run(category));

            _solrOperations.Commit();
        }
    }
}