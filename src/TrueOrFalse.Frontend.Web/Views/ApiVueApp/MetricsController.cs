using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiVueApp;
public class MetricsController(
QuestionViewRepository _questionViewRepository,
CategoryViewRepo _categoryViewRepo,
SessionUser _sessionUser) : Controller
{
    public readonly record struct GetAllDataResponse(int RegistrationsCount,
        int LoginCount,
        int CreatedPrivatizedTopicCount,
        int CreatedPublicTopicCount,
        int TodayTopicViews,
        int TodayQuestionViews,
        List<ViewsResult> ViewsQuestions,
        List<ViewsResult> ViewsTopics,
        List<ViewsResult> AnnualLogins,
        List<ViewsResult> AnnualRegistrations,
        List<ViewsResult> AnnualPublicCreatedTopics,
        List<ViewsResult> AnnualPrivateCreatedTopics
    );

    public readonly record struct ViewsResult(DateTime DateTime, int Views);

    [AccessOnlyAsLoggedIn]
    public GetAllDataResponse GetAllData()
    {
        if (!_sessionUser.IsInstallationAdmin)
            return new GetAllDataResponse();

        //Users
        var allUsers = EntityCache.GetAllUsers();
        var lastYearLogins = allUsers
            .Where(u => u.LastLogin.HasValue && u.LastLogin.Value.Date > DateTime.Now.Date.AddDays(-365))
            .ToList();

        var annualLogins = lastYearLogins
            .GroupBy(u => new { Year = u.LastLogin.Value.Year, Month = u.LastLogin.Value.Month })
            .Select(g => new ViewsResult(
                new DateTime(g.Key.Year, g.Key.Month, 1),
                g.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var todayLogins = allUsers
            .Where(DateTimeUtils.IsLastLoginToday);
        var todayRegistrations = allUsers.Where(DateTimeUtils.IsRegisterToday);

        var lastYearRegistrations = allUsers
            .Where(u => u.DateCreated.Date > DateTime.Now.Date.AddDays(-365))
            .ToList();

        var annualRegistrations = lastYearRegistrations
            .GroupBy(u => new { Year = u.DateCreated.Year, Month = u.DateCreated.Month })
            .Select(g => new ViewsResult(
                new DateTime(g.Key.Year, g.Key.Month, 1),
                g.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        //Topics
        var allCategories = EntityCache.GetAllCategoriesList();
        var allPublicCategories = allCategories.Where(u => u.IsVisible);
        var lastYearPublicCreatedTopics = allPublicCategories
            .Where(u => u.DateCreated.Date > DateTime.Now.Date.AddDays(-365))
            .ToList();

        var annualPublicCreatedTopics = lastYearPublicCreatedTopics
            .GroupBy(u => new { Year = u.DateCreated.Year, Month = u.DateCreated.Month })
            .Select(g => new ViewsResult(
                new DateTime(g.Key.Year, g.Key.Month, 1),
                g.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var publicTodayCreatedTopics = lastYearPublicCreatedTopics
            .Where(u => u.DateCreated.Date == DateTime.Now.Date);

        var allPrivateCreatedTopics = allCategories.Where(u => u.IsVisible == false);
        var lastYearPrivateCreatedTopics = allPrivateCreatedTopics
            .Where(u => u.DateCreated.Date > DateTime.Now.Date.AddDays(-365))
            .ToList();
        var annualPrivateCreatedTopics = lastYearPrivateCreatedTopics
            .GroupBy(u => new { Year = u.DateCreated.Year, Month = u.DateCreated.Month })
            .Select(g => new ViewsResult(
                new DateTime(g.Key.Year, g.Key.Month, 1),
                g.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var privateTodayCreatedTopics = allPrivateCreatedTopics
            .Where(DateTimeUtils.IsToday);

        var topicLastYearViews = _categoryViewRepo.GetViewsForLastNDays(365);
        var topicLastYearViewsResult = topicLastYearViews
            .Select(q => new ViewsResult(q.Key, q.Value))
            .ToList();
        var topicTodayViews = topicLastYearViewsResult.SingleOrDefault(t => t.DateTime.Date == DateTime.Now.Date).Views;

        //Questions
        var questionviewsLastYear = _questionViewRepository.GetViewsForLastNDays(365);
        var questionviewsLastYearResult = questionviewsLastYear
            .Select(q => new ViewsResult(q.Key, q.Value))
            .ToList();
        var questionTodayViews = questionviewsLastYearResult.SingleOrDefault(t => t.DateTime.Date == DateTime.Now.Date).Views;

        return new GetAllDataResponse
        {
            RegistrationsCount = todayRegistrations.Count(),
            LoginCount = todayLogins.Count(),
            CreatedPrivatizedTopicCount = privateTodayCreatedTopics.Count(),
            CreatedPublicTopicCount = publicTodayCreatedTopics.Count(),
            TodayTopicViews = topicTodayViews,
            TodayQuestionViews = questionTodayViews,
            ViewsQuestions = questionviewsLastYearResult,
            ViewsTopics = topicLastYearViewsResult,
            AnnualLogins = annualLogins,
            AnnualRegistrations = annualRegistrations,
            AnnualPublicCreatedTopics = annualPublicCreatedTopics,
            AnnualPrivateCreatedTopics = annualPrivateCreatedTopics
        };
    }
}
