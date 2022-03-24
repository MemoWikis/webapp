using System;
using System.Linq;

public class CategoryAuthorUpdater
{
    public static void UpdateAll()
    {
        var categories = Sl.CategoryRepo.GetAll();
        foreach (var category in categories)
        {
            Update(category);
        }
    }

    public static void Update(Category category)
    {
        var authors = Sl.CategoryChangeRepo.GetAuthorsOfCategory(category.Id).Distinct();
        category.AuthorIds = String.Join(",", authors.Select(a => a.Id).ToArray());
        Sl.CategoryRepo.Update(category);
    }
}

