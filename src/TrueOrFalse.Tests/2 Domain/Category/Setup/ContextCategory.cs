using System.Collections.Generic;
using System.Linq;
using BDDish.Model;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Tests
{
    public class ContextCategory : IRegisterAsInstancePerLifetime
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ContextUser _contextUser = ContextUser.New();
        
        public List<Category> All = new List<Category>();

        public ContextCategory(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _contextUser.Add("Context Category" ).Persist();
        }

        public static ContextCategory New()
        {
            return BaseTest.Resolve<ContextCategory>();
        }

        public ContextCategory Add(string categoryName)
        {
            Category category;
            if (_categoryRepository.Exists(categoryName))
                category = _categoryRepository.GetByName(categoryName);
            else
            {
                category = new Category(categoryName);
                category.Creator = _contextUser.All.First();
                _categoryRepository.Create(category);
            }

            All.Add(category);
            return this;
        }

        public ContextCategory Persist()
        {
            foreach(var cat in All)
                _categoryRepository.Create(cat);

            return this;
        }
    }
}
