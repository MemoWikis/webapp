using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public static class CategoryListExtensions
    {
        public static Category ByName(this IEnumerable<Category> categories, string name)
        {
            return categories.First(c => c.Name == name);
        }
    }
}
