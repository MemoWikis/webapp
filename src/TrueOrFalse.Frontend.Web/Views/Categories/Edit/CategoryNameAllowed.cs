using System.Collections.Generic;
using System.Linq;
public class CategoryNameAllowed
{
    public IList<Category> ExistingCategories { get; private set; }

    public bool Yes(Category category, CategoryRepository categoryRepository)
    {
        return Yes(category.Name, category.Type, categoryRepository);
    }

    public bool No(Category category, CategoryRepository categoryRepository)
    {
        return !Yes(category, categoryRepository);
    }

    private bool Yes(string categoryName, CategoryType type, CategoryRepository categoryRepository)
    {
        var typesToTest = new[] { CategoryType.Standard, CategoryType.Magazine, CategoryType.Daily };
        if (typesToTest.All(t => type != t))
            return true;

        ExistingCategories = categoryRepository.GetByName(categoryName);

        return ExistingCategories.All(c => c.Type != type);
    }

    public bool ForbiddenWords(string categoryName)
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
                return true;
        }

        return false;
    }
}