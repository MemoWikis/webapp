public class ActivityCalendarLoader(
    AnswerRepo _answerRepo,
    ExtendedUserCache _extendedUserCache) : IRegisterAsInstancePerLifetime
{
    public readonly record struct Activity(DateTime Day, int Count);

    public readonly record struct ActivityCalendar(IList<Activity> Activity);

    public ActivityCalendar GetForUser(int userId, int? pageId = null)
    {
        if (pageId.HasValue)
        {
            return GetFromDatabase(userId, pageId.Value);
        }

        return GetFromCache(userId);
    }

    private ActivityCalendar GetFromCache(int userId)
    {
        var user = _extendedUserCache.GetUser(userId);
        var startDate = DateTime.Today.AddDays(-364);

        var activity = user.ActivityCounts
            .Where(kvp => kvp.Key >= startDate)
            .OrderBy(kvp => kvp.Key)
            .Select(kvp => new Activity(kvp.Key, kvp.Value))
            .ToList();

        return new ActivityCalendar(activity);
    }

    private ActivityCalendar GetFromDatabase(int userId, int pageId)
    {
        var startDate = DateTime.Today.AddDays(-364);
        var dailyCounts = _answerRepo.GetDailyActivityForUserOnPage(userId, pageId, startDate);

        var activity = dailyCounts
            .Select(d => new Activity(d.Day, (int)d.Count))
            .ToList();

        return new ActivityCalendar(activity);
    }
}
