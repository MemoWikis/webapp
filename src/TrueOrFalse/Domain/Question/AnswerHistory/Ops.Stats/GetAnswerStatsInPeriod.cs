using System;
using NHibernate;

namespace TrueOrFalse
{
    public class GetAnswerStatsInPeriod : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public GetAnswerStatsInPeriod(ISession session){
            _session = session;
        }

        public GetAnswerStatsInPeriodResult RunForThisWeek(int userId){
            return Run(userId, DateTimeUtils.FirstDayOfThisWeek(), DateTime.Now);
        }
        
        public GetAnswerStatsInPeriodResult RunForPreviousWeek(int userId){
            return Run(userId, DateTimeUtils.FirstDayOfPreviousWeek(), DateTimeUtils.FirstDayOfThisWeek());
        }

        public GetAnswerStatsInPeriodResult RunForThisMonth(int userId){
            return Run(userId, DateTimeUtils.FirstDayOfThisMonth(), DateTime.Now);
        }

        public GetAnswerStatsInPeriodResult RunForPreviousMonth(int userId){
            return Run(userId, DateTimeUtils.FirstDayOfPreviousMonth(), DateTimeUtils.FirstDayOfThisMonth());
        }

        public GetAnswerStatsInPeriodResult Run(int userId, DateTime from, DateTime to)
        {
            var query = "SELECT " +
                        " COUNT(ah) as total," +
                        " SUM(AnswerredCorrectly) as totalCorrect " +
                        "FROM AnswerHistory ah " +
                        "WHERE UserId = " + userId + " " +
                        "AND DateCreated >= '" + from.ToString("yyy-MM-dd HH:mm:ss") + "' " +
                        "AND DateCreated < '" + to.ToString("yyy-MM-dd HH:mm:ss") + "'";

            var result = (object[])_session.CreateQuery(query).UniqueResult();

            return new GetAnswerStatsInPeriodResult{
                    TotalAnswers = Convert.ToInt32(result[0]),
                    TotalTrueAnswers = Convert.ToInt32(result[1])};
        }
    }
}
