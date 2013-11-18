using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.Search
{
    public class ToCategorytSolrMap
    {
        public static CategorySolrMap Run(Category category)
        {
            var result = new CategorySolrMap();
            result.Id = category.Id;
            result.Name = category.Name;
            result.Description = category.Description;
            result.CreatorId = category.Creator.Id;

            return result;
        }
    }
}
