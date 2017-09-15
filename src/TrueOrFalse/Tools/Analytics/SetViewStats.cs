using System;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;

public class SetViewStats
{
    private static DateTime _goLive = new DateTime(2016,10,11);
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
        var setViews = Sl.R<SetViewRepo>().GetViewsPerDay(setId, true).Where(a => a.DateTime >= _goLive);
        result.SetDetailViewsTotal = setViews.Sum(x => x.Value);
        result.SetDetailViewsLast7Days = setViews.Where(x => x.DateTime.Date < _ago0 && x.DateTime.Date >= _ago7D).Sum(x => x.Value);
        result.SetDetailViewsLast30Days = setViews.Where(x => x.DateTime.Date < _ago0 && x.DateTime.Date >= _ago30D).Sum(x => x.Value);
        result.SetDetailViewsPrec30Days = setViews.Where(x => x.DateTime.Date < _ago30D && x.DateTime.Date >= _ago60D).Sum(x => x.Value);

        var questionIds = set.QuestionsInSet.Select(x => x.Question.Id).ToList();

        var questionViews = Sl.R<QuestionViewRepository>().GetViewsPerDayForSetOfQuestions(questionIds).Where(a => a.DateTime >= _goLive);
        result.QuestionsViewsTotal = questionViews.Sum(x => x.Value);
        result.QuestionsViewsLast7Days = questionViews.Where(x => x.DateTime.Date < _ago0 && x.DateTime.Date >= _ago7D).Sum(x => x.Value);
        result.QuestionsViewsLast30Days = questionViews.Where(x => x.DateTime.Date < _ago0 && x.DateTime.Date >= _ago30D).Sum(x => x.Value);
        result.QuestionsViewsPrec30Days = questionViews.Where(x => x.DateTime.Date < _ago30D && x.DateTime.Date >= _ago60D).Sum(x => x.Value);

        result.QuestionsAnswersTotal = Sl.R<AnswerRepo>()
            .Query
            .Where(a => a.Question.Id.IsIn(questionIds.ToArray()))
            .And(a => a.DateCreated >= _goLive)
            .RowCount();

        result.LearningSessionsTotal = Sl.R<LearningSessionRepo>().GetNumberOfSessionsForSet(setId);

        result.DatesTotal = (int)Sl.Resolve<ISession>()
            .CreateSQLQuery("SELECT count(*) FROM date_to_sets WHERE Set_id =" + setId)
            .UniqueResult<long>();

        return result;
    }
        
}