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
    public readonly record struct GetAllDataResponse(
        int TodaysLoginCount,
        List<ViewsResult> MonthlyLoginsOfPastYear,
        List<ViewsResult> DailyLoginsOfPastYear,

        int TodaysRegistrationCount,
        List<ViewsResult> MonthlyRegistrationsOfPastYear,
        List<ViewsResult> DailyRegistrationsOfPastYear,

        int TodaysPublicTopicCreatedCount,
        List<ViewsResult> MonthlyPublicCreatedTopicsOfPastYear,

        int CreatedPrivateTopicCount,
        List<ViewsResult> MonthlyPrivateCreatedTopicsOfPastYear,

        int TodaysTopicViewCount,
        List<ViewsResult> TopicViewsOfPastYear,

        int TodaysQuestionViewCount,
        List<ViewsResult> QuestionViewsOfPastYear
    );
    public readonly record struct ViewsResult(DateTime DateTime, int Views);

    [AccessOnlyAsLoggedIn]
    public GetAllDataResponse GetAllData()
    {
        if (!_sessionUser.IsInstallationAdmin)
            return new GetAllDataResponse();

        //Users
        var (todaysLoginCount, dailyLoginsOfPastYear, monthlyLoginsOfPastYear) = GetLoginCounts();
        var (todaysRegistrationCount, dailyRegistrationsOfPastYear, monthlyRegistrationsOfPastYear) = GetRegistrationCounts();

        //Topics
        var (todaysPublicTopicCreatedCount, monthlyPublicCreatedTopicsOfPastYear) = GetPublicTopicCounts();
        var (todaysPrivateTopicCreatedCount, monthlyPrivateCreatedTopicsOfPastYear) = GetPrivateTopicsCounts();

        //TopicViews
        var (todaysTopicViewCount, topicViewsOfPastYear) = GetTopicViews();

        //Questions
        var (todaysQuestionViewCount, questionViewsOfPastYear) = GetQuestionViews();

        return new GetAllDataResponse
        {
            TodaysLoginCount = todaysLoginCount,
            MonthlyLoginsOfPastYear = monthlyLoginsOfPastYear,
            DailyLoginsOfPastYear = dailyLoginsOfPastYear,

            TodaysRegistrationCount = todaysRegistrationCount,
            MonthlyRegistrationsOfPastYear = monthlyRegistrationsOfPastYear,
            DailyRegistrationsOfPastYear = dailyRegistrationsOfPastYear,

            TodaysPublicTopicCreatedCount = todaysPublicTopicCreatedCount,
            MonthlyPublicCreatedTopicsOfPastYear = monthlyPublicCreatedTopicsOfPastYear,

            CreatedPrivateTopicCount = todaysPrivateTopicCreatedCount,
            MonthlyPrivateCreatedTopicsOfPastYear = monthlyPrivateCreatedTopicsOfPastYear,

            TodaysTopicViewCount = todaysTopicViewCount,
            TopicViewsOfPastYear = topicViewsOfPastYear,

            TodaysQuestionViewCount = todaysQuestionViewCount,
            QuestionViewsOfPastYear = questionViewsOfPastYear
        };
    }

    private (int todaysLoginCount, List<ViewsResult> dailyLoginsOfPastYear, List<ViewsResult> monthlyLoginsOfPastYear) GetLoginCounts()
    {
        var allUsers = EntityCache.GetAllUsers();

        var todaysLoginCount = allUsers.Count(DateTimeUtils.IsLastLoginToday);

        var lastYearLogins = allUsers
            .Where(u => u.LastLogin.HasValue && u.LastLogin.Value.Date > DateTime.Now.Date.AddDays(-365))
            .ToList();

        var startDate = DateTime.Now.Date.AddDays(-365);
        var endDate = DateTime.Now.Date;

        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(d => startDate.AddDays(d));

        var dailyLoginsOfPastYear = dateRange
            .GroupJoin(
                lastYearLogins,
                date => date,
                u => new DateTime(u.LastLogin.Value.Year, u.LastLogin.Value.Month, u.LastLogin.Value.Day),
                (date, logins) => new ViewsResult(date, logins.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var monthRange = Enumerable.Range(0, 12)
            .Select(m => startDate.AddMonths(m));

        var monthlyLoginsOfPastYear = monthRange
            .GroupJoin(
                lastYearLogins,
                month => new { month.Year, month.Month },
                u => new { u.LastLogin.Value.Year, Month = u.LastLogin.Value.Month },
                (month, logins) => new ViewsResult(new DateTime(month.Year, month.Month, 1), logins.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();


        return (todaysLoginCount, dailyLoginsOfPastYear, monthlyLoginsOfPastYear);
    }

    private (int todaysRegistrationCount, List<ViewsResult> dailyRegistrationsOfPastYear, List<ViewsResult> monthlyRegistrationsOfPastYear) GetRegistrationCounts()
    {
        var allUsers = EntityCache.GetAllUsers();

        var todaysRegistrationCount = allUsers.Count(DateTimeUtils.IsRegisterToday);

        var lastYearRegistrations = allUsers
            .Where(u => u.DateCreated.Date > DateTime.Now.Date.AddDays(-365))
            .ToList();

        var startDate = DateTime.Now.Date.AddDays(-365);
        var endDate = DateTime.Now.Date;

        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(d => startDate.AddDays(d));

        var dailyRegistrationsOfPastYear = dateRange
            .GroupJoin(
                lastYearRegistrations,
                date => date,
                u => u.DateCreated.Date,
                (date, registrations) => new ViewsResult(date, registrations.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var monthRange = Enumerable.Range(0, 12)
            .Select(m => startDate.AddMonths(m));

        var monthlyRegistrationsOfPastYear = monthRange
            .GroupJoin(
                lastYearRegistrations,
                month => new { month.Year, month.Month },
                u => new { u.DateCreated.Year, Month = u.DateCreated.Month },
                (month, registrations) => new ViewsResult(new DateTime(month.Year, month.Month, 1), registrations.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        return (todaysRegistrationCount, dailyRegistrationsOfPastYear, monthlyRegistrationsOfPastYear);
    }

    private (int todaysPublicTopicCreatedCount, List<ViewsResult> monthlyPublicCreatedTopicsOfPastYear) GetPublicTopicCounts()
    {
        var topics = EntityCache.GetAllCategoriesList();
        var publicTopics = topics.Where(u => u.IsPublic);
        var lastYearPublicCreatedTopics = publicTopics
            .Where(u => u.DateCreated.Date > DateTime.Now.Date.AddDays(-365))
            .ToList();

        var startDate = DateTime.Now.Date.AddDays(-365);
        var monthRange = Enumerable.Range(0, 12)
            .Select(m => startDate.AddMonths(m));

        var monthlyPublicCreatedTopicsOfPastYear = monthRange
            .GroupJoin(
                lastYearPublicCreatedTopics,
                month => new { month.Year, month.Month },
                u => new { u.DateCreated.Year, Month = u.DateCreated.Month },
                (month, topics) => new ViewsResult(new DateTime(month.Year, month.Month, 1), topics.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var todaysPublicTopicCreatedCount = lastYearPublicCreatedTopics.Count(u => u.DateCreated.Date == DateTime.Now.Date);

        return (todaysPublicTopicCreatedCount, monthlyPublicCreatedTopicsOfPastYear);
    }

    private (int todaysPrivateTopicCreatedCount, List<ViewsResult> monthlyPrivateCreatedTopicsOfPastYear) GetPrivateTopicsCounts()
    {
        var topics = EntityCache.GetAllCategoriesList();
        var privateTopics = topics.Where(u => u.IsPublic == false);
        var lastYearPrivateCreatedTopics = privateTopics
            .Where(u => u.DateCreated.Date > DateTime.Now.Date.AddDays(-365))
            .ToList();

        var startDate = DateTime.Now.Date.AddDays(-365);
        var monthRange = Enumerable.Range(0, 12)
            .Select(m => startDate.AddMonths(m));

        var monthlyPrivateCreatedTopicsOfPastYear = monthRange
            .GroupJoin(
                lastYearPrivateCreatedTopics,
                month => new { month.Year, month.Month },
                u => new { u.DateCreated.Year, Month = u.DateCreated.Month },
                (month, topics) => new ViewsResult(new DateTime(month.Year, month.Month, 1), topics.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var todaysPrivateTopicCreatedCount = lastYearPrivateCreatedTopics.Count(u => u.DateCreated.Date == DateTime.Now.Date);

        return (todaysPrivateTopicCreatedCount, monthlyPrivateCreatedTopicsOfPastYear);
    }

    private (int todaysTopicViews, List<ViewsResult> topicViewsOfPastYear) GetTopicViews()
    {
        var topicViewsOfPastYear = _categoryViewRepo.GetViewsForLastNDays(365);

        var startDate = DateTime.Now.Date.AddDays(-365);
        var endDate = DateTime.Now.Date;

        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(d => startDate.AddDays(d));

        var sortedTopicViewsOfPastYear = dateRange
            .GroupJoin(
                topicViewsOfPastYear,
                date => date,
                u => u.Key.Date,
                (date, views) => new ViewsResult(date, views.Sum(v => v.Value)))
            .OrderBy(v => v.DateTime)
            .ToList();

        var todaysTopicViews = sortedTopicViewsOfPastYear.SingleOrDefault(t => t.DateTime.Date == DateTime.Now.Date).Views;

        return (todaysTopicViews, sortedTopicViewsOfPastYear);
    }

    private (int todaysQuestionViews, List<ViewsResult> questionViewsOfPastYear) GetQuestionViews()
    {
        var questionViewsOfPastYear = _questionViewRepository.GetViewsForLastNDays(365);

        var startDate = DateTime.Now.Date.AddDays(-365);
        var endDate = DateTime.Now.Date;

        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(d => startDate.AddDays(d));

        var sortedQuestionViewsOfPastYear = dateRange
            .GroupJoin(
                questionViewsOfPastYear,
                date => date,
                u => u.Key.Date,
                (date, views) => new ViewsResult(date, views.Sum(v => v.Value)))
            .OrderBy(v => v.DateTime)
            .ToList();

        var todaysQuestionViews = sortedQuestionViewsOfPastYear.SingleOrDefault(t => t.DateTime.Date == DateTime.Now.Date).Views;

        return (todaysQuestionViews, sortedQuestionViewsOfPastYear);
    }
}
