using System;
using System.Linq;

public class CategoryAuthorUpdater
{
    public static void UpdateAll()
    {
        var categories = Sl.CategoryRepo.GetAllEager();
        foreach (var category in categories)
        {
            Update(category);
        }
    }

    public static void Update(Category category)
    {
        var authors = Sl.CategoryChangeRepo.GetAuthorsOfCategory(category.Id);
        authors = authors.GroupBy(p => p.Id).Select(grp => grp.FirstOrDefault()).ToList();
        var authorsToRemove = authors.Where(a => a.Id < 0).ToList();
        foreach (var authorToRemove in authorsToRemove)
        {
            authors.Remove(authorToRemove);
        }
        category.AuthorIds = String.Join(",", authors.Select(a => a.Id).ToArray());
        Sl.CategoryRepo.Update(category);
    }
}

