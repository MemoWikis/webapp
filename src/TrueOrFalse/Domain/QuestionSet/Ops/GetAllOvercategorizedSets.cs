using System.Collections.Generic;
using System.Linq;

public class GetAllOvercategorizedSets
{
    public static List<Set> Run()
    {
        var result = new List<Set>();
        var sets = Sl.SetRepo.GetAllEager();
        foreach (var set in sets)
        {
            var categories = set.Categories;
            foreach (var category in categories)
            {
                if (category.AggregatedCategories(false).Intersect(categories).Any())
                {
                    result.Add(set);
                    break;
                }

            }
        }
        return result;
    }
}
