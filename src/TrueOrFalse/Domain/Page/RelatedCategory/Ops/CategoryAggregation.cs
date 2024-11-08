class CategoryAggregation
{
    public static IList<Page> GetAggregatingAncestors(
        IList<Page> categories,
        PageRepository pageRepository)
    {
        return categories.SelectMany(c => pageRepository.GetIncludingCategories(c)).Distinct()
            .ToList();
    }
}