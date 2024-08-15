using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Domain;

namespace ApiVueApp;
public class OverviewController(
SessionUser _sessionUser) : Controller
{
    public readonly record struct OverviewRunJson(int RegistrationsCount,
        int LoginCount, 
        int CreatedPrivatizedTopicCount, 
        int CreatedPublicTopicCount,
        int ViewsTopics,
        int ViewsQuestions);

    [AccessOnlyAsAdmin]
    public OverviewRunJson GetAllData()
    {

        return new OverviewRunJson(); 

        var allUsers = EntityCache.GetAllUsers();
        var todayLogins = allUsers
            .Where(DateTimeUtils.IsLastLoginToday);
        var todayRegistrations = allUsers.Where(DateTimeUtils.IsRegisterToday);
        var userCache = EntityCache.GetUserById(_sessionUser.UserId); 

        var allCategories = EntityCache.GetAllCategoriesList();
        var publicCreated = allCategories
            .Where(DateTimeUtils.IsToday)
            .Where(u => u.IsVisible);
        var privateCreated = allCategories
            .Where(DateTimeUtils.IsToday)
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
