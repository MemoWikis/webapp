using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Util;
using Seedworks.Lib;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Web;

public class StatisticsModel : BaseModel
{
    private ISession _session;
    private int _memuchoId = 26;

    public UIMessage Message;

    public DateTime Since = DateTime.Now.AddDays(-31);
    public DateTime SinceGoLive = new DateTime(2016,10,11);

    public IList<User> Users;
    public IEnumerable<IGrouping<DateTime, User>> NewUsersGroupedByRegistrationDate;

    public IList<QuestionsCreatedPerDayResult> QuestionsCreatedPerDayResults;
    public IList<QuestionsCreatedPerDayResult> QuestionsExistingPerDayResults;

    public IList<UsageStatisticsOfLoggedInUsersResult> UsageStats;


    public StatisticsModel()
    {
        _session = Sl.R<ISession>();

        Users = _session
            .QueryOver<User>()
            .OrderBy(u => u.DateCreated).Asc
            .List();

        NewUsersGroupedByRegistrationDate = Users
            .Where(u => u.DateCreated.Date >= Since)
            .GroupBy(u => u.DateCreated.Date);


        QuestionsCreatedPerDayResults = _session
            .QueryOver<Question>()
            .Where(q => q.DateCreated.Date >= SinceGoLive)
            .List()
            .GroupBy(q => q.DateCreated.Date)
            .Select(r => new QuestionsCreatedPerDayResult
            {
                DateTime = r.Key,
                CountByMemucho = r.Count(q => q.Creator.Id == _memuchoId),
                CountByOthers = r.Count(q => q.Creator.Id != _memuchoId)
            })
            .ToList();

        while (QuestionsCreatedPerDayResults.Last().DateTime.Date < DateTime.Now.Date)
            QuestionsCreatedPerDayResults.Add(new QuestionsCreatedPerDayResult
            {
                DateTime = QuestionsCreatedPerDayResults.Last().DateTime.Date.AddDays(1),
                CountByOthers = 0,
                CountByMemucho = 0
            });

        var questionCountSoFarMemucho = _session
            .QueryOver<Question>()
            .Where(q => q.Creator.Id == _memuchoId)
            .And(q => q.DateCreated.Date < SinceGoLive)
            .List()
            .Count;
        var questionCountSoFarOthers = _session
            .QueryOver<Question>()
            .Where(q => q.Creator.Id != _memuchoId)
            .And(q => q.DateCreated.Date < SinceGoLive)
            .List()
            .Count;

        QuestionsExistingPerDayResults = new List<QuestionsCreatedPerDayResult>();
        QuestionsCreatedPerDayResults
            .OrderBy(q => q.DateTime)
            .ForEach(q =>
                {
                    questionCountSoFarMemucho += q.CountByMemucho;
                    questionCountSoFarOthers += q.CountByOthers;
                    QuestionsExistingPerDayResults.Add(new QuestionsCreatedPerDayResult
                    {
                        DateTime = q.DateTime,
                        CountByMemucho = questionCountSoFarMemucho,
                        CountByOthers = questionCountSoFarOthers
                    });
                });
        QuestionsCreatedPerDayResults = QuestionsCreatedPerDayResults.Where(q => q.DateTime >= Since).ToList();

        /* Usage Stats */
        FillUsageStatistics();

        
    }

    public void FillUsageStatistics()
    {
        var excludedUserIds = _session
            .QueryOver<User>()
            .Where(u => u.IsInstallationAdmin)
            .Select(u => u.Id)
            .List<int>();
        excludedUserIds.Add(-1);

        /* Part 1: How often did a logged in user (answer a questions | create a date | ...) on each day */

        var questionsAnsweredCount = _session
            .QueryOver<Answer>()
            //.Where(Restrictions.Not(Restrictions.In("UserId", excludedUserIds.ToList())))
            .WhereRestrictionOn(v => v.UserId).Not.IsIn((ICollection)excludedUserIds)
            .And(a => a.DateCreated.Date >= Since)
            .And(a => a.AnswerredCorrectly != AnswerCorrectness.IsView)
            .List()
            .GroupBy(a => a.DateCreated.Date)
            .Select(r => new TypeDateTimeInt
            {
                DateTime = r.Key,
                Int = r.Count()
            })
            .ToList();

        var questionsViewedCount = _session
            .QueryOver<QuestionView>()
            .WhereRestrictionOn(v => v.UserId).Not.IsIn((ICollection)excludedUserIds)
            .And(v => v.DateCreated.Date >= Since)
            .List()
            .GroupBy(v => v.DateCreated.Date)
            .Select(r => new TypeDateTimeInt
            {
                DateTime = r.Key,
                Int = r.Count()
            })
            .ToList();

        var learningSessionsStartedCount = _session
            .QueryOver<LearningSession>()
            .WhereRestrictionOn(l => l.User.Id).Not.IsIn((ICollection) excludedUserIds)
            .And(l => l.DateCreated.Date >= Since)
            .List()
            .GroupBy(l => l.DateCreated.Date)
            .Select(r => new TypeDateTimeInt
            {
                DateTime = r.Key,
                Int = r.Count()
            })
            .ToList();

        var datesCreatedCount = _session
            .QueryOver<Date>()
            .WhereRestrictionOn(d => d.User.Id).Not.IsIn((ICollection)excludedUserIds)
            .And(d => d.DateCreated.Date >= Since)
            .List()
            .GroupBy(d => d.DateCreated.Date)
            .Select(r => new TypeDateTimeInt
            {
                DateTime = r.Key,
                Int = r.Count()
            })
            .ToList();


        /* Part 2: How many different users have (answered Questions | created date | ...) on each day */
        // to check plausibility: "SELECT * from questionview Where UserId NOT IN (-1, 2, 25, 26, 33, 72, 75, 77) AND DateCreated > '2016-12-18';"

        var usersThatAnsweredQuestionCount = new List<TypeDateTimeInt>();
        _session.QueryOver<Answer>()
                .WhereRestrictionOn(a => a.UserId).Not.IsIn((ICollection)excludedUserIds)
                .And(a => a.DateCreated.Date >= Since)
                .List()
                .GroupBy(a => a.DateCreated.Date)
                .ForEach(u =>
                {
                    usersThatAnsweredQuestionCount.Add(new TypeDateTimeInt
                    {
                        DateTime = u.Key,
                        Int = u.Select(s => s.UserId).Distinct().Count()
                    });
                });

        var usersThatViewedQuestionCount = new List<TypeDateTimeInt>();
        _session.QueryOver<QuestionView>()
                .WhereRestrictionOn(v => v.UserId).Not.IsIn((ICollection)excludedUserIds)
                .And(v => v.DateCreated.Date >= Since)
                .List()
                .GroupBy(v => v.DateCreated.Date)
                .ForEach(u =>
                {
                    usersThatViewedQuestionCount.Add(new TypeDateTimeInt
                    {
                        DateTime = u.Key,
                        Int = u.Select(s => s.UserId).Distinct().Count()
                    });
                });

        var usersThatStartedLearningSessionCount = new List<TypeDateTimeInt>();
        _session.QueryOver<LearningSession>()
                .WhereRestrictionOn(l => l.User.Id).Not.IsIn((ICollection)excludedUserIds)
                .And(l => l.DateCreated.Date >= Since)
                .List()
                .GroupBy(l => l.DateCreated.Date)
                .ForEach(u =>
                {
                    usersThatStartedLearningSessionCount.Add(new TypeDateTimeInt
                    {
                        DateTime = u.Key,
                        Int = u.Select(s => s.User.Id).Distinct().Count()
                    });
                });

        var usersThatCreatedDateCount = new List<TypeDateTimeInt>();
        _session.QueryOver<Date>()
                .WhereRestrictionOn(l => l.User.Id).Not.IsIn((ICollection)excludedUserIds)
                .And(l => l.DateCreated.Date >= Since)
                .List()
                .GroupBy(l => l.DateCreated.Date)
                .ForEach(u =>
                {
                    usersThatCreatedDateCount.Add(new TypeDateTimeInt
                    {
                        DateTime = u.Key,
                        Int = u.Select(s => s.User.Id).Distinct().Count()
                    });
                });


        /* merge single usage statistics into condensed list */

        UsageStats = new List<UsageStatisticsOfLoggedInUsersResult>();
        var curDay = Since.Date;
        while (curDay <= DateTime.Now.Date)
        {
            UsageStats.Add(new UsageStatisticsOfLoggedInUsersResult
            {
                DateTime = curDay,
                QuestionsAnsweredCount = (questionsAnsweredCount.Find(i => i.DateTime == curDay) == null) ? 0 : questionsAnsweredCount.Find(i => i.DateTime == curDay).Int,
                QuestionsViewedCount = (questionsViewedCount.Find(i => i.DateTime == curDay) == null) ? 0 : questionsViewedCount.Find(i => i.DateTime == curDay).Int,
                LearningSessionsStartedCount = (learningSessionsStartedCount.Find(i => i.DateTime == curDay) == null) ? 0 : learningSessionsStartedCount.Find(i => i.DateTime == curDay).Int,
                DatesCreatedCount = (datesCreatedCount.Find(i => i.DateTime == curDay) == null) ? 0 : datesCreatedCount.Find(i => i.DateTime == curDay).Int,

                UsersThatAnsweredQuestionCount = (usersThatAnsweredQuestionCount.Find(i => i.DateTime == curDay) == null) ? 0 : usersThatAnsweredQuestionCount.Find(i => i.DateTime == curDay).Int,
                UsersThatViewedQuestionCount = (usersThatViewedQuestionCount.Find(i => i.DateTime == curDay) == null) ? 0 : usersThatViewedQuestionCount.Find(i => i.DateTime == curDay).Int,
                UsersThatStartedLearningSessionCount = (usersThatStartedLearningSessionCount.Find(i => i.DateTime == curDay) == null) ? 0 : usersThatStartedLearningSessionCount.Find(i => i.DateTime == curDay).Int,
                UsersThatCreatedDateCount = (usersThatCreatedDateCount.Find(i => i.DateTime == curDay) == null) ? 0 : usersThatCreatedDateCount.Find(i => i.DateTime == curDay).Int,
            });
            curDay = curDay.AddDays(1);
        }
    }

}