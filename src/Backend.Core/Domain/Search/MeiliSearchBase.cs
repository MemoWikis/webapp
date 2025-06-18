public class MeilisearchBase
{
    protected int _count = 20;

    public bool IsReloadRequired(int searchResultCount, int resultCount)
    {
        return searchResultCount == _count && resultCount < 5;
    }
}