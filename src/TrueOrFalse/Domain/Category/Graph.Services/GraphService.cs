using System.Collections.Generic;
using System.Linq;

public class GraphService
{
    public static IList<Category> GetAllParents(int categoryId) => 
        GetAllParents(Sl.CategoryRepo.GetById(categoryId));

    public static IList<Category> GetAllParents(Category category)
    {
        category = category == null ? new Category() : category;

        var currentGeneration = category.ParentCategories();
        var previousGeneration = new List<Category>();
        var parents = new List<Category>();

        while (currentGeneration.Count > 0)
        {
            parents.AddRange(currentGeneration);

            foreach (var currentCategory in currentGeneration)
            {
                var directParents = currentCategory.ParentCategories();
                if (directParents.Count > 0)
                {
                    previousGeneration.AddRange(directParents);
                }
            }

            currentGeneration = previousGeneration.Except(parents).Where(c => c.Id != category.Id).Distinct().ToList();
            previousGeneration = new List<Category>();
        }

        return parents;
    }
}
