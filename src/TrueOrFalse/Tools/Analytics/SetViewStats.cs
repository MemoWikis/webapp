using System;
using System.Linq;
using NHibernate.Criterion;

public class SetViewStats
{
    private static DateTime _ago0 = DateTime.Now.Date;
    private static DateTime _ago7D = DateTime.Now.Date.AddDays(-7);
    private static DateTime _ago30D = DateTime.Now.Date.AddDays(-30);
    private static DateTime _ago60D = DateTime.Now.Date.AddDays(-60);

    public static SetViewStatsResult GetForId(int setId)
    {
        var set = Sl.Resolve<SetRepo>().GetById(setId);
        var result = new SetViewStatsResult
        {
            SetId = setId,
            SetName = set.Name,
            Created = set.DateCreated
        };
        var setViews = Sl.R<SetViewRepo>().GetViewsPerDay(setId);
        result.SetDetailViewsTotal = setViews.Sum(x => x.Value);
        result.SetDetailViewsLast7Days = setViews.Where(x => x.DateTime.Date < _ago0 && x.DateTime.Date >= _ago7D).Sum(x => x.Value);
        result.SetDetailViewsLast30Days = setViews.Where(x => x.DateTime.Date < _ago0 && x.DateTime.Date >= _ago30D).Sum(x => x.Value);
        result.SetDetailViewsPrec30Days = setViews.Where(x => x.DateTime.Date < _ago30D && x.DateTime.Date >= _ago60D).Sum(x => x.Value);

        var questionIds = set.QuestionsInSet.Select(x => x.Question.Id).ToList();
        //var questionViews = Sl.R<QuestionViewRepository>()
        //    .Query
        //    .Where(x => x.QuestionId.IsIn(questionIds.ToArray()))
        //    .List()
        //    .GroupBy(x => x.DateCreated.Date)
        //    .Select(x => new AmountPerDay
        //    {
        //        DateTime = x.Key,
        //        Value = x.Count()
        //    })
        //    .ToList(); // == 347
        var questionViews = Sl.R<QuestionViewRepository>().GetViewsPerDayForSetOfQuestions(questionIds);
        result.QuestionsViewsTotal = questionViews.Sum(x => x.Value);
        result.QuestionsViewsLast7Days = questionViews.Where(x => x.DateTime.Date < _ago0 && x.DateTime.Date >= _ago7D).Sum(x => x.Value);
        result.QuestionsViewsLast30Days = questionViews.Where(x => x.DateTime.Date < _ago0 && x.DateTime.Date >= _ago30D).Sum(x => x.Value);
        result.QuestionsViewsPrec30Days = questionViews.Where(x => x.DateTime.Date < _ago30D && x.DateTime.Date >= _ago60D).Sum(x => x.Value);

        result.QuestionsAnswersTotal = Sl.R<AnswerRepo>()
            .Query
            .Where(x => x.Question.Id.IsIn(questionIds.ToArray()))
            .RowCount();

        //result.LearningSessionsTotal = Sl.R<LearningSessionRepo>()
        //    .Query
        //    .Where(x => (x.SetToLearn.Id == setId) || (x.SetsToLearnIdsString.Contains(setId.ToString())))
        //    .RowCount();

        //result.DatesTotal = Sl.R<DateRepo>()
        //    .Query
        //    .Where(d => d.Sets.Select(s => s.Id).ToString().Contains(setId.ToString()))
        //    .RowCount();
        //shorter: select * from date_to_sets where Set_id = 14;

        return result;
    }
        
}