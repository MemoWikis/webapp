using TrueOrFalse.Search;

    public interface IGlobalSearch
    {
        GlobalSearchResult Go(string term, string type);
        GlobalSearchResult GoAllCategories(string term, int[] categoriesToFilter = null);
    }
