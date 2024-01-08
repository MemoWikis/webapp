public class Crumbtrail(CategoryCacheItem _current, CategoryCacheItem _root)
{
    public CrumbtrailItem Root = new(_root);
    public CrumbtrailItem Current = new(_current);
    public IList<CrumbtrailItem> Items = new List<CrumbtrailItem>();

    public bool Rootfound;

    public void Add(CategoryCacheItem category)
    {
        if (Root.IsEqual(category))
            Rootfound = true;

        Items.Add(new CrumbtrailItem(category));
    }
}