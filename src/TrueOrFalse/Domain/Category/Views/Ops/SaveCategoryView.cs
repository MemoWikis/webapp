public class SaveCategoryView
{
    public static void Run(CategoryCacheItem categoryCacheItem, User user)
    {
        var userAgent = UserAgent.Get();

        if (IsCrawlerRequest.Yes(userAgent))
            return;

        var category = new Category()
        {
            Id = categoryCacheItem.Id,
            Name = categoryCacheItem.Name
        }; 

        Sl.R<CategoryViewRepo>().Create(new CategoryView
        {
            Category = category,
            User = user,
            UserAgent = userAgent
        });
    }
}