public class Crumbtrail(PageCacheItem _current, PageCacheItem _root)
{
    public CrumbtrailItem Root = new(_root);
    public CrumbtrailItem Current = new(_current);
    public IList<CrumbtrailItem> Items = new List<CrumbtrailItem>();

    public bool Rootfound;

    public void Add(PageCacheItem page)
    {
        if (Root.IsEqual(page))
            Rootfound = true;

        Items.Add(new CrumbtrailItem(page));
    }
}