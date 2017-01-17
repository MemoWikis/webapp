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
    public IList<TypeDateTimeInt> Tempi;


    public StatisticsModel()
    {
        _session = Sl.R<ISession>();

        Users = _session
            .QueryOver<User>()
            .OrderBy(u => u.DateCreated).Asc
            .List();

        NewUsersGroupedByRegistrationDate = Users
            .Where(u => u.DateCreated > Since)
            .GroupBy(u => u.DateCreated.Date);

        var questions = _session
            .QueryOver<Question>()
            .Where(q => q.DateCreated > Since)
            .OrderBy(q => q.DateCreated).Asc
            .List();

        QuestionsCreatedPerDayResults = _session
            .QueryOver<Question>()
            .Where(q => q.DateCreated > SinceGoLive)
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
            .And(q => q.DateCreated < SinceGoLive)
            .List()
            .Count;
        var questionCountSoFarOthers = _session
            .QueryOver<Question>()
            .Where(q => q.Creator.Id != _memuchoId)
            .And(q => q.DateCreated < SinceGoLive)
            .List()
            .Count;

        QuestionsExistingPerDayResults = new List<QuestionsCreatedPerDayResult>();
        QuestionsCreatedPerDayResults.OrderBy(q => q.DateTime).ForEach(q =>
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

        var questionsAnsweredCount = _session
            .QueryOver<Answer>()
            //.Where(Restrictions.Not(Restrictions.In("UserId", excludedUserIds.ToList())))
            .WhereRestrictionOn(v => v.UserId).Not.IsIn((ICollection)excludedUserIds)
            .And(a => a.DateCreated > Since)
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
            .And(v => v.DateCreated > Since)
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
            .And(l => l.DateCreated > Since)
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
            .And(d => d.DateCreated > Since)
            .List()
            .GroupBy(d => d.DateCreated.Date)
            .Select(r => new TypeDateTimeInt
            {
                DateTime = r.Key,
                Int = r.Count()
            })
            .ToList();

        /* Part 2: How many different users have (answered Questions | created date | ...) on each day */

        var usersThatAnsweredQuestionCount = new List<TypeDateTimeInt>();
        _session
            .QueryOver<Answer>()
            .WhereRestrictionOn(l => l.UserId).Not.IsIn((ICollection) excludedUserIds)
            .And(l => l.DateCreated > Since)
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

        Tempi = usersThatAnsweredQuestionCount;

    }

}