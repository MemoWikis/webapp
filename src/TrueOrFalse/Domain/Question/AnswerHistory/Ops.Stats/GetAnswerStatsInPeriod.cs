using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class GetAnswerStatsInPeriod : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public GetAnswerStatsInPeriod(ISession session){
        _session = session;
    }

    public GetAnswerStatsInPeriodResult RunForThisWeek(int userId){
        return Run(userId, DateTimeUtils.FirstDayOfThisWeek(), DateTime.Now)[0];
    }

    public GetAnswerStatsInPeriodResult RunForThisMonth(int userId){
        return Run(userId, DateTimeUtils.FirstDayOfThisMonth(), DateTime.Now)[0];
    }

    public GetAnswerStatsInPeriodResult RunForThisYear(int userId){
        return Run(userId, DateTimeUtils.FirstDayOfThisYear(), DateTime.Now)[0];
    }

    public GetAnswerStatsInPeriodResult RunForLastWeek(int userId){
        return Run(userId, DateTimeUtils.FirstDayOfLastWeek(), DateTimeUtils.FirstDayOfThisWeek())[0];
    }

    public GetAnswerStatsInPeriodResult RunForLastMonth(int userId){
        return Run(userId, DateTimeUtils.FirstDayOfLastMonth(), DateTimeUtils.FirstDayOfThisMonth())[0];
    }

    public GetAnswerStatsInPeriodResult RunForLastYear(int userId){
        return Run(userId, DateTimeUtils.FirstDayOfLastYear(), DateTimeUtils.FirstDayOfThisYear())[0];
    }

    public GetAnswerStatsInPeriodResult Run(int userId)
    {
        return Run(userId, null, null)[0];
    }

    public IList<GetAnswerStatsInPeriodResult> GetLast30Days(int userId)
    {
        var result = new List<GetAnswerStatsInPeriodResult>();
        var rowsFromDb = Run(userId, DateTime.Now.AddDays(-30).Date, DateTime.Now, groupByDate: true);
        for (var i = 0; i < 30; i++)
        {
            var entryDate = DateTime.Now.AddDays(-(30 - i)).Date;
            var fromDb = rowsFromDb.FirstOrDefault(x => entryDate == x.DateTime.Date);

            if (fromDb != null)
                result.Add(fromDb);
            else
                result.Add(new GetAnswerStatsInPeriodResult
                {
                    DateTime = entryDate,
                    TotalAnswers = 0,
                    TotalTrueAnswers = 0
                });
        }

        return result;
    }

    public IList<GetAnswerStatsInPeriodResult> Run(
        int userId, 
        DateTime? from, DateTime? to,
        bool groupByDate = false)
    {
        var query = "SELECT " +
                    " COUNT(ah) as total," +
                    " SUM(CASE WHEN AnswerredCorrectly = 1 THEN 1 WHEN AnswerredCorrectly = 2 THEN 1 ELSE 0 END) as totalCorrect, " +
                    " Date(DateCreated) " +
                    "FROM AnswerHistory ah " +
                    "WHERE UserId = " + userId + " ";

        if (from.HasValue && to.HasValue)
        {
            query +=
                    "AND DateCreated >= '" + ((DateTime)from).ToString("yyy-MM-dd HH:mm:ss") + "' " +
                    "AND DateCreated < '" + ((DateTime)to).ToString("yyy-MM-dd HH:mm:ss") + "'";    
        }

        if (groupByDate)
        {
            query += "GROUP BY DATE(DateCreated) ";
        }

        var rows = (IList<object>)_session.CreateQuery(query).List();

        var result = new List<GetAnswerStatsInPeriodResult>();
        foreach (var row in rows)
        {
            var values = (object[]) row;
            result.Add(new GetAnswerStatsInPeriodResult
            {
                TotalAnswers = Convert.ToInt32(values[0]),
                TotalTrueAnswers = Convert.ToInt32(values[1]),
                DateTime = Convert.ToDateTime(values[2])
            });
        }

        return result;
    }
}