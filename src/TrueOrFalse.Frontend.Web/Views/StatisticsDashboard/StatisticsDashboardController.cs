using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Util;


    public class StatisticsDashboardController : Controller
    {
        [HttpGet]
        public int[] GetCreatedQuestionsInTimeWindow(int amount, string interval)
        {
            DateTime since;
            int _memuchoId = 26;


            switch (interval)
            {
                case "day":
                {
                    since = DateTime.Now.AddDays(-amount);
                    break;
                }
                case "week":
                {
                    since = DateTime.Now.AddDays(-amount*7);
                    break;
                }
                case "month":
                {
                    since = DateTime.Now.AddMonths(-amount);
                    break;
                }
                case "year":
                {
                    since = DateTime.Now.AddYears(-amount);
                    break;
                }
                default:
                    return null;
            }
            var _session = Sl.R<ISession>();
            var QuestionsCreatedPerDayResults = _session
                .QueryOver<Question>()
                .Where(q => q.DateCreated.Date >= since)
                .List()
                .GroupBy(q => q.DateCreated.Date)
                .Select(r => new QuestionsCreatedPerDayResult
                {
                    DateTime = r.Key,
                    CountByMemucho = r.Count(q => q.Creator.Id == _memuchoId),
                    CountByOthers = r.Count(q => q.Creator.Id != _memuchoId),
                })
                .ToList();
            var result = QuestionsCreatedPerDayResults.Select(i => i.TotalCount).ToArray();
            return result;
        }
    }