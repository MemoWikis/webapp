using System.Collections.Generic;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Tests
{
    public class ContextCategory : IRegisterAsInstancePerLifetime
    {
        private readonly CategoryRepository _categoryRepository;
        
        public List<Category> AllCategories = new List<Category>();

        public ContextCategory(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public static ContextCategory New()
        {
            return BaseTest.Resolve<ContextCategory>();
        }

        public ContextCategory Add(string categoryName)
        {
            AllCategories.Add(new Category(categoryName));
            return this;
        }

        public ContextCategory Persist()
        {
            foreach(var cat in AllCategories)
                _categoryRepository.Create(cat);

            return this;
        }
    }
}
