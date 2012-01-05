using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class CategoryExists : IRegisterAsInstancePerLifetime
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryExists(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <remarks>Extension point for caching...</remarks>
        public bool Yes(string categoryName)
        {
            return _categoryRepository.Exists(categoryName);
        }
    }
}
