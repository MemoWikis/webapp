using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;

public class GraphService
{
    public static IList<Category> GetAllParents(int categoryId) =>
        GetAllParents(Sl.CategoryRepo.GetById(categoryId));

    public static IList<Category> GetAllParents(Category category)
    {
        category = category == null ? new Category() : category;

        var currentGeneration = category.ParentCategories(); // get ParentCategorys
        var previousGeneration = new List<Category>(); // new List is empty
        var parents = new List<Category>();  // new List is empty
                                             //

        while (currentGeneration.Count > 0) // 
        {
            parents.AddRange(currentGeneration);

            foreach (var currentCategory in currentGeneration)  //go through Parents 
            {
                var directParents = currentCategory.ParentCategories(); // Get from all parents the Parents
                if (directParents.Count > 0) // go through ParentParents
                {
                    previousGeneration.AddRange(directParents); // Add the ParentParents
                }
            }

            currentGeneration = previousGeneration.Except(parents).Where(c => c.Id != category.Id).Distinct().ToList(); // ParentParents except the Parents and parentparentcategory.id is non equal categoryId 
            previousGeneration = new List<Category>(); // clear list
            // return in While loop
        }

        return parents;
    }

    public static void AutomaticInclusionFromSubthemes(Category category)
    {
        var parentsFromParentCategories = GraphService.GetAllParents(category);
        if (parentsFromParentCategories.Count != 0)
        {
            foreach (var parentCategory in parentsFromParentCategories)
            {
                ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(parentCategory);
            }
        }
    }
}
