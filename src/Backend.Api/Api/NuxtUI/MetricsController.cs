public class MetricsController(
QuestionViewRepository _questionViewRepository,
PageViewRepo pageViewRepo,
SessionUser _sessionUser) : ApiBaseController
{
    public readonly record struct GetAllDataResponse(
        int TodaysActiveUserCount,
        List<ViewsResult> MonthlyActiveUsersOfPastYear,
        List<ViewsResult> DailyActiveUsersOfPastYear,
        int TodaysRegistrationCount,
        List<ViewsResult> MonthlyRegistrationsOfPastYear,
        List<ViewsResult> DailyRegistrationsOfPastYear,
        int TodaysPublicPageCreatedCount,
        List<ViewsResult> MonthlyPublicCreatedPagesOfPastYear,
        int CreatedPrivatePageCount,
        List<ViewsResult> MonthlyPrivateCreatedPagesOfPastYear,
        List<ViewsResult> DailyPrivateCreatedPagesOfPastYear,
        int TodaysPageViewCount,
        List<ViewsResult> PageViewsOfPastYear,
        int TodaysQuestionViewCount,
        List<ViewsResult> QuestionViewsOfPastYear,
        int TodaysPublishedQuestionCount,
        List<ViewsResult> MonthlyPublishedQuestionsOfPastYear,
        List<ViewsResult> DailyPublishedQuestionsOfPastYear,
        int TodaysPublicQuestionCreatedCount,
        List<ViewsResult> MonthlyPublicCreatedQuestionsOfPastYear,
        int TodaysPrivateQuestionCreatedCount,
        List<ViewsResult> MonthlyPrivateCreatedQuestionsOfPastYear);

    public readonly record struct ViewsResult(DateTime DateTime, int Views);

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public GetAllDataResponse GetAllData()
    {
        if (!_sessionUser.IsInstallationAdmin)
            return new GetAllDataResponse();

        //Users
        var (todaysActiveUserCount, dailyActiveUsersOfPastYear, monthlyActiveUsersOfPastYear) = GetActiveUserCounts();
        var (todaysRegistrationCount, dailyRegistrationsOfPastYear, monthlyRegistrationsOfPastYear) = GetRegistrationCounts();

        //Pages
        var (todaysPublicPageCreatedCount, monthlyPublicCreatedPagesOfPastYear) = GetPublicPageCounts();
        var (todaysPrivatePageCreatedCount, monthlyPrivateCreatedPagesOfPastYear, dailyPrivateCreatedPagesOfPastYear) = GetPrivatePagesCounts();

        //PageViews
        var (todaysPageViewCount, pageViewsOfPastYear) = GetPageViews();

        //Questions
        var (todaysPublishedQuestionCount, monthlyPublishedQuestionsOfPastYear, dailyPublishedQuestionsOfPastYear) = GetPublishedQuestionCount();
        var (todaysPublicQuestionCreatedCount, monthlyPublicCreatedQuestionsOfPastYear) = GetPublicQuestionCount();
        var (todaysPrivateQuestionCreatedCount, monthlyPrivateCreatedQuestionsOfPastYear) = GetPrivateQuestionCount();

        //QuestionViews
        var (todaysQuestionViewCount, questionViewsOfPastYear) = GetQuestionViews();

        return new GetAllDataResponse
        {
            TodaysActiveUserCount = todaysActiveUserCount,
            MonthlyActiveUsersOfPastYear = monthlyActiveUsersOfPastYear,
            DailyActiveUsersOfPastYear = dailyActiveUsersOfPastYear,

            TodaysRegistrationCount = todaysRegistrationCount,
            MonthlyRegistrationsOfPastYear = monthlyRegistrationsOfPastYear,
            DailyRegistrationsOfPastYear = dailyRegistrationsOfPastYear,

            TodaysPublicPageCreatedCount = todaysPublicPageCreatedCount,
            MonthlyPublicCreatedPagesOfPastYear = monthlyPublicCreatedPagesOfPastYear,

            CreatedPrivatePageCount = todaysPrivatePageCreatedCount,
            MonthlyPrivateCreatedPagesOfPastYear = monthlyPrivateCreatedPagesOfPastYear,
            DailyPrivateCreatedPagesOfPastYear = dailyPrivateCreatedPagesOfPastYear,

            TodaysPageViewCount = todaysPageViewCount,
            PageViewsOfPastYear = pageViewsOfPastYear,

            TodaysQuestionViewCount = todaysQuestionViewCount,
            QuestionViewsOfPastYear = questionViewsOfPastYear,

            TodaysPublishedQuestionCount = todaysPublishedQuestionCount,
            MonthlyPublishedQuestionsOfPastYear = monthlyPublishedQuestionsOfPastYear,
            DailyPublishedQuestionsOfPastYear = dailyPublishedQuestionsOfPastYear,

            TodaysPublicQuestionCreatedCount = todaysPublicQuestionCreatedCount,
            MonthlyPublicCreatedQuestionsOfPastYear = monthlyPublicCreatedQuestionsOfPastYear,

            TodaysPrivateQuestionCreatedCount = todaysPrivateQuestionCreatedCount,
            MonthlyPrivateCreatedQuestionsOfPastYear = monthlyPrivateCreatedQuestionsOfPastYear
        };
    }

    private (int todaysActiveUserCount, List<ViewsResult> dailyActiveUsersOfPastYear, List<ViewsResult> monthlyActiveUsersOfPastYear) GetActiveUserCounts()
    {
        var activeUserCountForPastYear = pageViewRepo.GetActiveUserCountForPastNDays(365);

        var todaysActiveUserCount = activeUserCountForPastYear.TryGetValue(DateTime.Now.Date, out var todaysCount)
            ? todaysCount
            : 0;

        var activeUsersOfPastYear = activeUserCountForPastYear
            .Where(v => v.Key > DateTime.Now.Date.AddDays(-365))
            .OrderBy(v => v.Key)
            .ToList();

        var startDate = DateTime.Now.Date.AddDays(-365);
        var endDate = DateTime.Now.Date;

        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(d => startDate.AddDays(d));

        var dailyActiveUsersOfPastYear = dateRange
            .GroupJoin(
                activeUsersOfPastYear,
                date => date,
                v => v.Key,
                (date, ActiveUsers) => new ViewsResult(date, ActiveUsers.FirstOrDefault().Value))
            .OrderBy(v => v.DateTime)
            .ToList();

        var monthRange = Enumerable.Range(0, 12)
            .Select(m => startDate.AddMonths(m));

        var monthlyActiveUsersOfPastYear = monthRange
            .GroupJoin(
                activeUsersOfPastYear,
                month => new { month.Year, month.Month },
                v => new { v.Key.Year, v.Key.Month },
                (month, ActiveUsers) => new ViewsResult(new DateTime(month.Year, month.Month, 1), ActiveUsers.Sum(a => a.Value)))
            .OrderBy(v => v.DateTime)
            .ToList();

        return (todaysActiveUserCount, dailyActiveUsersOfPastYear, monthlyActiveUsersOfPastYear);
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

    private (int todaysPublicPageCreatedCount, List<ViewsResult> monthlyPublicCreatedPagesOfPastYear) GetPublicPageCounts()
    {
        var pages = EntityCache.GetAllPagesList();
        var publicPages = pages.Where(u => u.IsPublic);
        var lastYearPublicCreatedPages = publicPages
            .Where(u => u.DateCreated.Date > DateTime.Now.Date.AddDays(-365))
            .ToList();

        var startDate = DateTime.Now.Date.AddDays(-365);
        var monthRange = Enumerable.Range(0, 12)
            .Select(m => startDate.AddMonths(m));

        var monthlyPublicCreatedPagesOfPastYear = monthRange
            .GroupJoin(
                lastYearPublicCreatedPages,
                month => new { month.Year, month.Month },
                u => new { u.DateCreated.Year, Month = u.DateCreated.Month },
                (month, pages) => new ViewsResult(new DateTime(month.Year, month.Month, 1), pages.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var todaysPublicPageCreatedCount = lastYearPublicCreatedPages.Count(u => u.DateCreated.Date == DateTime.Now.Date);

        return (todaysPublicPageCreatedCount, monthlyPublicCreatedPagesOfPastYear);
    }

    private (int todaysPrivatePageCreatedCount, List<ViewsResult> monthlyPrivateCreatedPagesOfPastYear, List<ViewsResult> dailyPrivateCreatedPagesOfPastYear) GetPrivatePagesCounts()
    {
        var pages = EntityCache.GetAllPagesList();
        var privatePages = pages.Where(u => u.IsPublic == false);
        var lastYearPrivateCreatedPages = privatePages
            .Where(u => u.DateCreated.Date > DateTime.Now.Date.AddDays(-365))
            .ToList();

        var startDate = DateTime.Now.Date.AddDays(-365);
        var endDate = DateTime.Now.Date;

        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(d => startDate.AddDays(d));

        var dailyPrivateCreatedPagesOfPastYear = dateRange
            .GroupJoin(
                lastYearPrivateCreatedPages,
                date => date,
                u => u.DateCreated.Date,
                (date, registrations) => new ViewsResult(date, registrations.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var monthRange = Enumerable.Range(0, 12)
            .Select(m => startDate.AddMonths(m));

        var monthlyPrivateCreatedPagesOfPastYear = monthRange
            .GroupJoin(
                lastYearPrivateCreatedPages,
                month => new { month.Year, month.Month },
                u => new { u.DateCreated.Year, Month = u.DateCreated.Month },
                (month, pages) => new ViewsResult(new DateTime(month.Year, month.Month, 1), pages.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var todaysPrivatePageCreatedCount = lastYearPrivateCreatedPages.Count(u => u.DateCreated.Date == DateTime.Now.Date);

        return (todaysPrivatePageCreatedCount, monthlyPrivateCreatedPagesOfPastYear, dailyPrivateCreatedPagesOfPastYear);
    }

    private (int todaysPublishedQuestionCount, List<ViewsResult> monthlyPublishedQuestionsOfPastYear, List<ViewsResult> dailyPublishedQuestionsOfPastYear) GetPublishedQuestionCount()
    {
        var questions = EntityCache.GetAllQuestions();
        var publicQuestions = questions.Where(q => q.IsPublic);
        var lastYearPublishedQuestions = publicQuestions
            .Where(q => q.LastPublishDate.Date > DateTime.Now.Date.AddDays(-365))
            .ToList();

        var startDate = DateTime.Now.Date.AddDays(-365);
        var endDate = DateTime.Now.Date;

        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(d => startDate.AddDays(d));

        var dailyPublishedQuestionsOfPastYear = dateRange
            .GroupJoin(
                lastYearPublishedQuestions,
                date => date,
                q => q.LastPublishDate.Date,
                (date, registrations) => new ViewsResult(date, registrations.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var monthRange = Enumerable.Range(0, 12)
            .Select(m => startDate.AddMonths(m));

        var monthlyPublishedQuestionsOfPastYear = monthRange
            .GroupJoin(
                lastYearPublishedQuestions,
                month => new { month.Year, month.Month },
                q => new { q.LastPublishDate.Year, Month = q.LastPublishDate.Month },
                (month, questions) => new ViewsResult(new DateTime(month.Year, month.Month, 1), questions.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var todaysPublishedQuestionCount = lastYearPublishedQuestions.Count(q => q.LastPublishDate.Date == DateTime.Now.Date);

        return (todaysPublishedQuestionCount, monthlyPublishedQuestionsOfPastYear, dailyPublishedQuestionsOfPastYear);
    }

    private (int todaysPublicQuestionCreatedCount, List<ViewsResult> monthlyPublishedQuestionsOfPastYear) GetPublicQuestionCount()
    {
        var questions = EntityCache.GetAllQuestions();
        var publicQuestions = questions.Where(q => q.IsPublic);
        var lastYearPublicCreatedQuestions = publicQuestions
            .Where(u => u.DateCreated.Date > DateTime.Now.Date.AddDays(-365))
            .ToList();

        var startDate = DateTime.Now.Date.AddDays(-365);
        var monthRange = Enumerable.Range(0, 12)
            .Select(m => startDate.AddMonths(m));

        var monthlyPublicCreatedQuestionsOfPastYear = monthRange
            .GroupJoin(
                lastYearPublicCreatedQuestions,
                month => new { month.Year, month.Month },
                u => new { u.DateCreated.Year, Month = u.DateCreated.Month },
                (month, pages) => new ViewsResult(new DateTime(month.Year, month.Month, 1), pages.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var todaysPublicQuestionCreatedCount = lastYearPublicCreatedQuestions.Count(u => u.DateCreated.Date == DateTime.Now.Date);

        return (todaysPublicQuestionCreatedCount, monthlyPublicCreatedQuestionsOfPastYear);
    }

    private (int todaysPrivateQuestionCreatedCount, List<ViewsResult> monthlyPublishedQuestionsOfPastYear) GetPrivateQuestionCount()
    {
        var questions = EntityCache.GetAllQuestions();
        var privateQuestions = questions.Where(q => q.IsPublic == false);
        var lastYearPrivateCreatedQuestions = privateQuestions
            .Where(u => u.DateCreated.Date > DateTime.Now.Date.AddDays(-365))
            .ToList();

        var startDate = DateTime.Now.Date.AddDays(-365);
        var monthRange = Enumerable.Range(0, 12)
            .Select(m => startDate.AddMonths(m));

        var monthlyPrivateCreatedQuestionsOfPastYear = monthRange
            .GroupJoin(
                lastYearPrivateCreatedQuestions,
                month => new { month.Year, month.Month },
                u => new { u.DateCreated.Year, Month = u.DateCreated.Month },
                (month, pages) => new ViewsResult(new DateTime(month.Year, month.Month, 1), pages.Count()))
            .OrderBy(v => v.DateTime)
            .ToList();

        var todaysPrivateQuestionCreatedCount = lastYearPrivateCreatedQuestions.Count(u => u.DateCreated.Date == DateTime.Now.Date);

        return (todaysPrivateQuestionCreatedCount, monthlyPrivateCreatedQuestionsOfPastYear);
    }

    private (int todaysPageViews, List<ViewsResult> pageViewsOfPastYear) GetPageViews()
    {
        var pageViewsOfPastYear = pageViewRepo.GetViewsForPastNDays(365);

        var startDate = DateTime.Now.Date.AddDays(-365);
        var endDate = DateTime.Now.Date;

        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(d => startDate.AddDays(d));

        var sortedPageViewsOfPastYear = dateRange
            .GroupJoin(
                pageViewsOfPastYear,
                date => date,
                u => u.Key.Date,
                (date, views) => new ViewsResult(date, views.Sum(v => v.Value)))
            .OrderBy(v => v.DateTime)
            .ToList();

        var todaysPageViews = sortedPageViewsOfPastYear.SingleOrDefault(t => t.DateTime.Date == DateTime.Now.Date).Views;

        return (todaysPageViews, sortedPageViewsOfPastYear);
    }

    private (int todaysQuestionViews, List<ViewsResult> questionViewsOfPastYear) GetQuestionViews()
    {
        var questionViewsOfPastYear = _questionViewRepository.GetViewsForPastNDays(365);

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
