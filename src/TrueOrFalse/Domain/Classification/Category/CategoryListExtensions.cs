using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse
{
    public static class CategoryListExtensions
    {
        public static Category ByName(this IEnumerable<Category> categories, string name)
        {
            return categories.First(c => c.Name == name);
        }

        public static string GetValueByIndex(this IEnumerable<Category> categories, int index)
        {
            if (categories != null && categories.Count() > index)
                return categories.ToList()[index].Name;

            return "";
        }
    }
}
