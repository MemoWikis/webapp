using System.Collections.Generic;

public class ContentRecommendationResult
{
    public IList<Set> Sets = new List<Set>(); //Sets that match the content
    public IList<Category> Categories = new List<Category>(); //Categories that match the content
    public IList<Set> PopularSets = new List<Set>();

}
