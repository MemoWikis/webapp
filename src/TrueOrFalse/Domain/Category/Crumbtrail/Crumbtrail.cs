using System.Collections.Generic;
using System.Linq;

public class Crumbtrail
{
    public CrumbtrailItem Root;
    public CrumbtrailItem Current;
    public IList<CrumbtrailItem> Items = new List<CrumbtrailItem>();

    public Crumbtrail(CategoryCacheItem current, CategoryCacheItem root)
    {
        Current = new CrumbtrailItem(current);
        Root = new CrumbtrailItem(root);
    }

    public bool Rootfound;

    public string ToDebugString() => Items
                                         .Select(c => c.Text)
                                         .Aggregate((a, b) => a + " => " + b) 
                                     + " => [" + Current.Text + "]";

    public bool AlreadyAdded(CategoryCacheItem   category)
    {
        foreach (var item in Items)
            if (item.IsEqual(category))
                return true;

        return false;
    }

    public void Add(CategoryCacheItem category)
    {
        if (Root.IsEqual(category))
            Rootfound = true;

        Items.Add(new CrumbtrailItem(category));
    }
}
