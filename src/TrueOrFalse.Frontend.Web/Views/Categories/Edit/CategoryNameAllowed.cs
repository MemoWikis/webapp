using System.Collections.Generic;
using System.Linq;
using TrueOrFalse;

public class CategoryNameAllowed
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
        if (!testForbiddenWords(categoryName))
            return false;

        var typesToTest = new[] { CategoryType.Standard, CategoryType.Magazine, CategoryType.Daily };
        if (typesToTest.All(t => type != t))
            return true;

        ExistingCategories = ServiceLocator.Resolve<CategoryRepository>().GetByName(categoryName);

        return ExistingCategories.All(c => c.Type != type);

    }

    private bool testForbiddenWords(string categoryName)
    {
        var categoryNameProcessed = categoryName.Trim().ToLower();

        var forbiddenWords = new[]
        {
            "wissenszentrale", "kategorien", "fragen", "widgets", "ueber-memucho", "fuer-lehrer",
            "widget-beispiele", "widget-angebote-preislisten",
            "hilfe", "impressum", "imprint", "agb", "agbs", "jobs", "gemeinwohloekonomie", "team"
        };

        foreach (var fW in forbiddenWords)
        {
            if (fW.Equals(categoryNameProcessed))
                return false;
        }

        return true;
    }
}