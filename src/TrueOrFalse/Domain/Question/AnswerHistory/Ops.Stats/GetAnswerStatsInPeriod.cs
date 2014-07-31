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

        public GetAnswerStatsInPeriodResult RunForThisMonth(int userId){
            return Run(userId, DateTimeUtils.FirstDayOfThisMonth(), DateTime.Now);
        }

        public GetAnswerStatsInPeriodResult RunForThisYear(int userId){
            return Run(userId, DateTimeUtils.FirstDayOfThisYear(), DateTime.Now);
        }

        public GetAnswerStatsInPeriodResult RunForLastWeek(int userId){
            return Run(userId, DateTimeUtils.FirstDayOfLastWeek(), DateTimeUtils.FirstDayOfThisWeek());
        }

        public GetAnswerStatsInPeriodResult RunForLastMonth(int userId){
            return Run(userId, DateTimeUtils.FirstDayOfLastMonth(), DateTimeUtils.FirstDayOfThisMonth());
        }

        public GetAnswerStatsInPeriodResult RunForLastYear(int userId){
            return Run(userId, DateTimeUtils.FirstDayOfLastYear(), DateTimeUtils.FirstDayOfThisYear());
        }

        public GetAnswerStatsInPeriodResult Run(int userId)
        {
            return Run(userId, null, null);
        }

        public GetAnswerStatsInPeriodResult Run(int userId, DateTime? from, DateTime? to)
        {
            var query = "SELECT " +
                        " COUNT(ah) as total," +
                        " SUM(AnswerredCorrectly) as totalCorrect " +
                        "FROM AnswerHistory ah " +
                        "WHERE UserId = " + userId + " ";

            if (from.HasValue && to.HasValue)
            {
                query +=
                        "AND DateCreated >= '" + ((DateTime)from).ToString("yyy-MM-dd HH:mm:ss") + "' " +
                        "AND DateCreated < '" + ((DateTime)to).ToString("yyy-MM-dd HH:mm:ss") + "'";    
            }

            var result = (object[])_session.CreateQuery(query).UniqueResult();

            return new GetAnswerStatsInPeriodResult{
                    TotalAnswers = Convert.ToInt32(result[0]),
                    TotalTrueAnswers = Convert.ToInt32(result[1])};
        }
    }
}
