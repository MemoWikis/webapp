using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;
using NHibernate;
using NHibernate.Util;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Web;

public class StatisticsModel : BaseModel
{
    private ISession _session;
    private int _memuchoId = 26;

    public UIMessage Message;

    public DateTime Since = DateTime.Now.AddDays(-14);
    public DateTime SinceGoLive = new DateTime(2016,10,11);

    public IList<User> Users;
    public IEnumerable<IGrouping<DateTime, User>> NewUsersGroupedByRegistrationDate;

    public IList<QuestionsCreatedPerDayResult> QuestionsCreatedPerDayResults;
    public IList<QuestionsCreatedPerDayResult> QuestionsExistingPerDayResults;


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

    }

}