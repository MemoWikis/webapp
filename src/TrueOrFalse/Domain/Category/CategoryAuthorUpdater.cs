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
        category.AuthorIds = String.Join(",", authors.Select(a => a));
        var sql = "UPDATE category " +
        "SET AuthorIds = " + category.AuthorIds +
        " WHERE Id = " + category.Id + ";";

        Sl.CategoryRepo.UpdateAuthors(sql);

        Sl.CategoryRepo.Update(category);
    }
}