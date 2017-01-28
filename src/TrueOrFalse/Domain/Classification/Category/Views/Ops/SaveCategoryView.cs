public class SaveCategoryView
{
    public static void Run(Category category, User user)
    {
        var userAgent = UserAgent.Get();

        if (IsCrawlerRequest.Yes(userAgent))
            return;

        Sl.R<CategoryViewRepo>().Create(new CategoryView
        {
            Category = category,
            User = user,
            UserAgent = userAgent
        });
    }
}