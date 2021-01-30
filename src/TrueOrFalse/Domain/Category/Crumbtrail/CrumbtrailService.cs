using System.Linq;

public class CrumbtrailService
{
    public static Crumbtrail Get(Category category, Category root)
    {
        var result = new Crumbtrail(category, root);
        var parents = category.ParentCategories();
        foreach (var parent in parents) 
            AddParent(result, parent);

        result.Items = result.Items.Reverse().ToList();

        return result;
    }

    private static void AddParent(Crumbtrail crumbtrail, Category category)
    {
        if (crumbtrail.Rootfound)
            return;

        crumbtrail.Add(category);

        foreach (var currentCategory in category.ParentCategories())
        {
            if (crumbtrail.AlreadyAdded(currentCategory))
                return;

            AddParent(crumbtrail, currentCategory);
        } ;
    }
}
