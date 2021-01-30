using System.Linq;

public class CrumbtrailService
{
    public static Crumbtrail Get(Category category)
    {
        var result = new Crumbtrail(category);
        var parents = category.ParentCategories();
        foreach (var parent in parents) 
            Get(result, parent);

        result.Items = result.Items.Reverse().ToList();

        return result;
    }

    private static void Get(Crumbtrail crumbtrail, Category category)
    {
        crumbtrail.Items.Add(new CrumbtrailItem(category));

        foreach (var currentCategory in category.ParentCategories())
            Get(crumbtrail, currentCategory);
    }
}
