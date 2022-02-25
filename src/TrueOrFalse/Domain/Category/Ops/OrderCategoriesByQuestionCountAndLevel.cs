using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OrderCategoriesByQuestionCountAndLevel
{
    public static IList<Category> Run(IList<CategoryCacheItem> categories)
    {
        var output = new List<CategoryCacheItem>();
        categories = categories.OrderByDescending(c => c.CountQuestionsAggregated).ToList();

        foreach (var category in categories) //sort list of categories putting child categories after their parents
        {
            //before inserting, iterate over all categories already inserted to check, where this one should be inserted
            var insertAfterIndex = -1;
            foreach (var alreadySelectedCategory in output)
            {
                if (alreadySelectedCategory.AggregatedCategories().ContainsKey(category.Id))
                {
                    //check if more specific category was already added to insert this one there
                    if (insertAfterIndex == -1 || output.ElementAt(insertAfterIndex).AggregatedCategories().ContainsKey(alreadySelectedCategory.Id))
                    {
                        insertAfterIndex = output.IndexOf(alreadySelectedCategory);
                    }

                }
            }

            //check if next is subcat of allreadySelectedCat; while that is true, postpone the insert after last item of same/inferior level (to keep order by questioncount)
            var nextIndex = insertAfterIndex + 1;
            while (insertAfterIndex != -1
                   && output.Count > nextIndex
                   && output.ElementAt(insertAfterIndex).AggregatedCategories().ContainsKey(output.ElementAt(nextIndex).Id))
            {
                nextIndex++;
            }
            insertAfterIndex = nextIndex - 1;


            if (insertAfterIndex == -1)
                insertAfterIndex = output.Count - 1; //if category is not sub-topic of any already selected, then insert it at the end
            output.Insert(insertAfterIndex + 1, category);
        }

        var categoryList = Category.ToCategories(output).ToList();
        return categoryList;
    }
}
