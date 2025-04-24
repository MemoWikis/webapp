public class CrumbtrailItem(PageCacheItem page)
{
    public string Text = page.Name;
    public readonly PageCacheItem Page = page;

    public bool IsEqual(PageCacheItem page) => Page.Id == page.Id;
}