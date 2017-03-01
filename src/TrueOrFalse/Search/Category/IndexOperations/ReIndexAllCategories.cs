﻿using SolrNet;

namespace TrueOrFalse.Search
{
    public class ReIndexAllCategories : IRegisterAsInstancePerLifetime
    {
        private readonly CategoryRepository _categoryRepo;
        private readonly ISolrOperations<CategorySolrMap> _solrOperations;
        private readonly CategoryValuationRepo _categoryValuationRepo;

        public ReIndexAllCategories(
            CategoryRepository categoryRepo,
            ISolrOperations<CategorySolrMap> solrOperations,
            CategoryValuationRepo categoryValuationRepo)
        {
            _categoryRepo = categoryRepo;
            _solrOperations = solrOperations;
            _categoryValuationRepo = categoryValuationRepo;
        }

        public void Run()
        {
            _solrOperations.Delete(new SolrQuery("*:*")); //Delete all sets
            _solrOperations.Commit();
            
            foreach (var category in _categoryRepo.GetAll())
                _solrOperations.Add(ToCategorytSolrMap.Run(category, _categoryValuationRepo.GetBy(category.Id)));

            _solrOperations.Commit();
        }
    }
}