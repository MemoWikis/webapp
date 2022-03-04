using System.Collections.Generic;

public class ContentRecommendationResult
{
    public IList<CategoryCacheItem> Categories = new List<CategoryCacheItem>(); //Categories that match the content
    public IList<Set> PopularSets = new List<Set>();
}
