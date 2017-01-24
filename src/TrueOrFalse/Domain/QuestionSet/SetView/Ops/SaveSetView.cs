public class SaveSetView
{
    public static void Run(Set set, User user)
    {
        var userAgent = UserAgent.Get();

        if (IsCrawlerRequest.Yes(userAgent))
            return;

        Sl.R<SetViewRepo>().Create(new SetView
        {
            Set = set,
            User = user,
            UserAgent = userAgent
        });
    }
}