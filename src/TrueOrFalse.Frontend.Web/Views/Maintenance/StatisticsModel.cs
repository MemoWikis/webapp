using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;
using NHibernate;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Web;

public class StatisticsModel : BaseModel
{
    private ISession _session;

    public UIMessage Message;

    public DateTime Since = DateTime.Now.AddDays(-14);
    public DateTime SinceGoLive = new DateTime(2016,10,11);

    public IList<User> Users;
    public IEnumerable<IGrouping<DateTime, User>> NewUsersGroupedByRegistrationDate;

    public IEnumerable<QuestionsCreatedStatisticsResult> QuestionsCreatedResult;

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

        QuestionsCreatedResult = new List<QuestionsCreatedStatisticsResult>();
    }

}