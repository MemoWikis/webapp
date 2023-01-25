using System.Collections.Generic;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class SolrSearchIndexCategory : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<CategorySolrMap> _solrOperations;
        private CategoryValuationRepo __categoryValuationRepo;

        private CategoryValuationRepo _categoryValuationRepo =>
            __categoryValuationRepo ??
            (__categoryValuationRepo = Sl.Resolve<CategoryValuationRepo>());

        public SolrSearchIndexCategory(ISolrOperations<CategorySolrMap> solrOperations){
            _solrOperations = solrOperations;
        }

        public void Update(Category category)
        {
            _solrOperations.Add(ToCategorytSolrMap.Run(category, _categoryValuationRepo.GetBy(category.Id)));
            _solrOperations.Commit();
        }

        public void Update(IList<Category> categories)
        {
            foreach(var category in categories)
                _solrOperations.Add(ToCategorytSolrMap.Run(category, _categoryValuationRepo.GetBy(category.Id)));

            _solrOperations.Commit();
        }

        public void Delete(Category category)
        {
            _solrOperations.Delete(ToCategorytSolrMap.Run(category, _categoryValuationRepo.GetBy(category.Id)));
            _solrOperations.Commit();
        }
    }
}