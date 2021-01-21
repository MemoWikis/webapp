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
        //not used so far. If used, be aware that result includes AnswerCorrectness.IsView
        return Run(userId, DateTimeUtils.FirstDayOfThisWeek(), DateTime.Now)[0];
    }

    public GetAnswerStatsInPeriodResult RunForThisMonth(int userId){
        //not used so far. If used, be aware that result includes AnswerCorrectness.IsView
        return Run(userId, DateTimeUtils.FirstDayOfThisMonth(), DateTime.Now)[0];
    }

    public GetAnswerStatsInPeriodResult RunForThisYear(int userId){
        //not used so far. If used, be aware that result includes AnswerCorrectness.IsView
        return Run(userId, DateTimeUtils.FirstDayOfThisYear(), DateTime.Now)[0];
    }

    public GetAnswerStatsInPeriodResult RunForLastWeek(int userId){
        //not used so far. If used, be aware that result includes AnswerCorrectness.IsView
        return Run(userId, DateTimeUtils.FirstDayOfLastWeek(), DateTimeUtils.FirstDayOfThisWeek())[0];
    }

    public GetAnswerStatsInPeriodResult RunForLastMonth(int userId){
        //not used so far. If used, be aware that result includes AnswerCorrectness.IsView
        return Run(userId, DateTimeUtils.FirstDayOfLastMonth(), DateTimeUtils.FirstDayOfThisMonth())[0];
    }

    public GetAnswerStatsInPeriodResult RunForLastYear(int userId){
        //not used so far. If used, be aware that result includes AnswerCorrectness.IsView
        return Run(userId, DateTimeUtils.FirstDayOfLastYear(), DateTimeUtils.FirstDayOfThisYear())[0];
    }

    public GetAnswerStatsInPeriodResult Run(int userId)
    {
        //not used so far. If used, be aware that result includes AnswerCorrectness.IsView
        return Run(userId, null, null)[0];
    }

    public IList<GetAnswerStatsInPeriodResult> GetLast30Days(int userId)
    {
        var result = new List<GetAnswerStatsInPeriodResult>();
        var rowsFromDb = Run(userId, DateTime.Now.AddDays(-30).Date, DateTime.Now, groupByDate: true, excludeAnswerViews: true);
        for (var i = 1; i <= 30; i++)
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
        bool groupByDate = false,
        bool onlyLearningSessions = false,
        int? startHour = null,
        int? endHour = null,
        bool excludeAnswerViews = false)
    {
        var query = "SELECT " +
                    " COUNT(ah) as total," +
                    " SUM(CASE WHEN AnswerredCorrectly = 1 THEN 1 WHEN AnswerredCorrectly = 2 THEN 1 ELSE 0 END) as totalCorrect, " +
                    " Date(DateCreated) " +
                    "FROM Answer ah " +
                    "WHERE UserId = " + userId + " ";

        if (excludeAnswerViews)
            query += "AND AnswerredCorrectly != 3 ";

        if(startHour != null && endHour != null)
            query +=
                @" AND TIME(DateCreated) >= MAKETIME({0}, 0, 0)
                   AND TIME(DateCreated) <= MAKETIME({1}, 0, 0) ";

        if (from.HasValue && to.HasValue)
        {
            query += String.Format(
                    "AND DateCreated >= '" + ((DateTime)from).ToString("yyy-MM-dd HH:mm:ss") + "' " +
                    "AND DateCreated < '" + ((DateTime)to).ToString("yyy-MM-dd HH:mm:ss") + "'",
                        startHour, endHour);    
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