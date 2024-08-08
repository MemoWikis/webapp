using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Domain;

namespace ApiVueApp;
public class OverviewController : Controller
{
    public readonly record struct OverviewRunJson(int RegistrationsCount,
        int LoginCount, 
        int CreatedPrivatizedTopicCount, 
        int CreatedPublicTopicCount,
        int ViewsTopics,
        int ViewsQuestions);
    public OverviewRunJson GetAllData()
    {
        var allUsers = EntityCache.GetAllUsers();
        var todayLogins = allUsers
            .Where(DateTimeChecks.IsLastLoginToday);
        var todayRegistrations = allUsers.Where(DateTimeChecks.IsRegisterToday);

        var allCategories = EntityCache.GetAllCategoriesList();
        var publicCreated = allCategories
            .Where(DateTimeChecks.IsToday)
            .Where(u => u.IsVisible);
        var privateCreated = allCategories
            .Where(DateTimeChecks.IsToday)
            .Where(u => u.IsVisible == false);
        var allTopicTodayViews = allCategories.Sum(t => t.TodayViewCount);
        var allQuestionTodayViews = EntityCache.GetAllQuestions().Sum(t => t.TodayViewCount);

        return new OverviewRunJson(todayRegistrations.Count(),
            todayLogins.Count(), 
            privateCreated.Count(), 
            publicCreated.Count(),
            allTopicTodayViews, 
            allQuestionTodayViews);
    }
}
