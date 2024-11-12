class PageAggregation
{
    public static IList<Page> GetAggregatingAncestors(
        IList<Page> pages,
        PageRepository pageRepository)
    {
        return pages.SelectMany(c => pageRepository.GetIncludingPages(c)).Distinct()
            .ToList();
    }
}