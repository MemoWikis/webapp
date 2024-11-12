class PageAggregation
{
    public static IList<Page> GetAggregatingAncestors(
        IList<Page> categories,
        PageRepository pageRepository)
    {
        return categories.SelectMany(c => pageRepository.GetIncludingPages(c)).Distinct()
            .ToList();
    }
}