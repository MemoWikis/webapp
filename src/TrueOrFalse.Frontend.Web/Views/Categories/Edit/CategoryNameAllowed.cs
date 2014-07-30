using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;

public class CategoryNameAllowed : IRegisterAsInstancePerLifetime
{
    public IList<Category> ExistingCategories { get; private set; }

    public bool Yes(EditCategoryModel model, CategoryType type)
    {
        return Yes(model.Name, type);
    }

    public bool No(EditCategoryModel model, CategoryType type)
    {
        return !Yes(model.Name, type);
    }

    public bool Yes(Category category)
    {
        return Yes(category.Name, category.Type);
    }

    public bool No(Category category)
    {
        return !Yes(category);
    }

    private bool Yes(string categoryName, CategoryType type)
    {
        var typesToTest = new[] {CategoryType.Standard, CategoryType.Magazine, CategoryType.Daily};
        if (typesToTest.All(t => type != t))
            return true;

        ExistingCategories = ServiceLocator.Resolve<CategoryRepository>().GetByName(categoryName);

        return ExistingCategories.All(c => c.Type != type);        
    }
}