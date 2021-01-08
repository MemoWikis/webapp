using FluentNHibernate.Utils;
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

            foreach (var currentCategory in currentGeneration)  //go through Parents 
            {
                var directParents = EntityCache.GetCategory(currentCategory.Id).ParentCategories(); //Parents of parents are not eagerly loaded. Because of that, we get the parents using the cache.
                if (directParents.Count > 0) // go through ParentParents
                {
                    previousGeneration.AddRange(directParents); // Add the ParentParents
                }
            }

            currentGeneration = previousGeneration
                .Except(parents)
                .Where(c => c.Id != category.Id)
                .Distinct()
                .ToList(); // ParentParents except the Parents and parentparentcategory.id is non equal categoryId 

            previousGeneration = new List<Category>(); // clear list
        }
        return parents;
    }

    public static List<Category> GetLastWuwiChildrenFromCategories(int categoryId)
    {
        var childrenReverse = EntityCache.GetDescendants(categoryId);
        var lastChildren = childrenReverse
            .Where(c =>
                EntityCache.GetChildren(c.Id).Count == 0 &&
                c.IsInWishknowledge());

        return lastChildren.ToList();

    }

    public static IList<Category> GetAllPersonelCategoriesWithRealtions(int rootCategoryId, int userId = -1)
    {
        var rootCategory = Extensions.DeepClone(
            EntityCache.GetCategory(rootCategoryId, true));

        var children = Extensions.DeepClone(
             EntityCache.GetDescendants(rootCategory, true))
            .Distinct();

        var listWithUserPersonelCategories = new List<Category>();

        userId = userId == -1 ? Sl.CurrentUserId : userId;

        foreach (var child in children)
        {
            if (!Sl.CategoryValuationRepo.IsInWishKnowledge(child.Id, userId))
                continue;

            var parents = GetParentsFromCategory(child);
            var hasRootInParents = parents.Any(c => c.Id == rootCategoryId);
            child.CategoryRelations.Clear();
            listWithUserPersonelCategories.Add(child);

            while (parents.Count > 0)
            {
                var parent = parents.First();

                if (Sl.CategoryValuationRepo.IsInWishKnowledge(parent.Id, userId) || parent.Id == rootCategoryId && hasRootInParents)
                {
                    var categoryRelation = new CategoryRelation
                    {
                        CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
                        Category = child,
                        RelatedCategory = parent
                    };

                        var indexOfChild = listWithUserPersonelCategories.IndexOf(child);

                        if (listWithUserPersonelCategories[indexOfChild].CategoryRelations.All(cr =>
                            cr.RelatedCategory.Id != categoryRelation.RelatedCategory.Id)) // Not add if available
                            listWithUserPersonelCategories[indexOfChild]
                                .CategoryRelations
                                .Add(categoryRelation);

                        parents.Remove(parent);
                }
                else
                {
                    var currentParents = GetParentsFromCategory(parent);
                    parents.Remove(parent);

                    foreach (var cp in currentParents)
                    {
                        parents.Add(cp);
                    }

                    parents = parents.Distinct().ToList();
                }
            }
        }

        foreach (var listWithUserPersonelCategory in listWithUserPersonelCategories)
        {
            if (listWithUserPersonelCategory.CategoryRelations.Count == 0)
            {
                listWithUserPersonelCategory.CategoryRelations.Add(new CategoryRelation
                {
                    CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
                    RelatedCategory = rootCategory,
                    Category = listWithUserPersonelCategory
                });
            }
        }

        rootCategory.CategoryRelations = new List<CategoryRelation>();
        listWithUserPersonelCategories.Add(rootCategory);
            
        return listWithUserPersonelCategories;
    }

    private static List<Category> GetParentsFromCategory(Category category)
    {
        return category.CategoryRelations.Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildCategoryOf).Select(cr => cr.RelatedCategory).ToList();
    }

    public static IList<Category> GetAllPersonelCategoriesWithRealtions(Category category, int userId = -1) =>
        GetAllPersonelCategoriesWithRealtions(category.Id, userId);

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

    public static bool IsCategoryParentEqual(IList<Category> parent1 , IList<Category> parent2)
    {
        if (parent1 == null || parent2 == null)
        {
            Logg.r().Error("parent1 or parent2 have a NullReferenceException");
            return false; 
        }
           

        if (parent1.Count != parent2.Count)
            return false;

        if (parent1.Count == 0 && parent2.Count == 0)
            return true;

        var result = parent1.Where(p => !parent2.Any(p2 => p2.Id == p.Id)).Count();

        return result == 0;
    }

    public static bool IsCategoryRelationEqual(Category category1, Category category2)
    {
        if (category1 != null && category2 != null || category1.CategoryRelations != null && category2.CategoryRelations != null)
        {
            var relations1 = category1.CategoryRelations;
            var relations2 = category2.CategoryRelations;

            if (relations2.Count != relations1.Count)
                return false;

            if (relations2.Count == 0 && relations1.Count == 0)
                return true;

            var count = 0;

            var countVariousRelations = relations1.Where(r => !relations2.Any(r2 => r2.RelatedCategory.Id == r.RelatedCategory.Id && r2.Category.Id == r.Category.Id && r2.CategoryRelationType.ToString().Equals(r.CategoryRelationType.ToString()))).Count();
            return countVariousRelations == 0;
        }
        Logg.r().Error("Category or CategoryRelations have a NullReferenceException");
        return false;
    }
}
