using System;
using System.Linq;
using NHibernate;

public class ReputationCalc : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public const int PointsPerQuestionCreated = 1; //excluding private questions
    public const int PointsPerQuestionInOtherWishknowledge = 5;
    public const int PointsPerSetCreated = 2;
    public const int PointsPerSetInOtherWishknowledge = 10;
    public const int PointsPerDateCreatedVisible = 1;
    public const int PointsPerDateCopied = 5; //Own dates copied by others
    public const int PointsPerUserFollowingMe = 20;
    public const int PointsForPublicWishknowledge = 30;

    public ReputationCalc(ISession session){
        _session = session;
    }

    public ReputationCalcResult Run(User user)
    {
        var result = new ReputationCalcResult();
        result.User = new UserTinyModel(user);

        /*Calculate Reputation for Questions and Sets created */

        var createdQuestions = _session.QueryOver<Question>()
            .Where(q => q.Creator.Id == result.User.Id)
            .And(q => q.Visibility == QuestionVisibility.All)
            .List<Question>();
        result.ForQuestionsCreated = createdQuestions.Count * PointsPerQuestionCreated;

        var createdSets = _session.QueryOver<Set>()
            .Where(s => s.Creator.Id == result.User.Id)
            .RowCount();
        result.ForSetsCreated = createdSets*PointsPerSetCreated;

        /*Calculate Reputation for Questions, Sets, Categories in other user's wish knowledge */

        var countQuestionsInOtherWishknowledge = GetCountOfQuestionsInOtherPeoplesWishknowledge(user);
        result.ForQuestionsInOtherWishknowledge = countQuestionsInOtherWishknowledge * PointsPerQuestionInOtherWishknowledge;

        var countSetsInOtherWishknowledge = GetCountOfSetsInOtherPeoplesWishknowledge(user);
        result.ForSetsInOtherWishknowledge = countSetsInOtherWishknowledge * PointsPerSetInOtherWishknowledge;


        /* Calculate Reputation for Dates */
        var datesCreatedVisibleCount =
            _session.QueryOver<Date>()
                .Where(d => d.User == user)
                .And(d => d.Visibility == DateVisibility.InNetwork)
                .RowCount();
        var datesCopiedInstancesCount =
            _session.QueryOver<Date>().Where(d => d.User == user).List<Date>().Sum(d => d.CopiedInstances.Count);
        result.ForDatesCreatedVisible = datesCreatedVisibleCount * PointsPerDateCreatedVisible;
        result.ForDatesCopied = datesCopiedInstancesCount * PointsPerDateCopied;

        /* Calculate Reputation for other things */

        result.ForPublicWishknowledge = result.User.ShowWishKnowledge ? PointsForPublicWishknowledge : 0;
        result.ForUsersFollowingMe = _session.R<TotalFollowers>().Run(result.User.Id) * PointsPerUserFollowingMe;

        return result;
    }

    private int GetCountOfQuestionsInOtherPeoplesWishknowledge(User user)
    {
        var countQuestionsInOtherWishknowledge = _session.QueryOver<QuestionValuation>()
            .Where(qv => qv.User != user)
            .And(qv => qv.RelevancePersonal != -1)
            .JoinQueryOver<Question>(qv => qv.Question)
            .Where(q => q.Creator == user)
            .RowCount();
        return countQuestionsInOtherWishknowledge;
    }

    private int GetCountOfSetsInOtherPeoplesWishknowledge(User user)
    {
        //The following doesn't work, unfortunately (reason: Set is not a property of setValuation)
        //var countSetsInOtherWishknowledge = _session.QueryOver<SetValuation>()
        //    .Where(sv => sv.UserId != user.Id)
        //    .And(qv => qv.RelevancePersonal != -1)
        //    .Left.JoinAlias()
        //    .JoinQueryOver<Set>(sv => sv.Set)
        //    .Where(s => s.Creator == user)
        //    .RowCount();

        //this is the alternative by writing sql-code (it is working)
        var tinyUser = new UserTinyModel(user);
        var query =
            $@"SELECT count(*)
                FROM setvaluation sv
                LEFT JOIN questionset s
                ON sv.SetId = s.Id
                WHERE s.Creator_id = {tinyUser.Id}
                AND sv.UserId <> {tinyUser.Id}
                AND sv.RelevancePersonal <> -1";

        var countSetsInOtherWishknowledge = Convert.ToInt32(_session.CreateSQLQuery(query).UniqueResult());
        return countSetsInOtherWishknowledge;
    }

    private int GetCountOfCategoriesInOtherPeoplesWishknowledge(User user)
    {
        var query =
            $@"SELECT count(*)
                FROM categoryvaluation cv
                LEFT JOIN questionset s
                ON cv.CategoryId = s.Id
                WHERE s.Creator_id = {user.Id}
                AND sv.UserId <> {user.Id}
                AND sv.RelevancePersonal <> -1";

        var countSetsInOtherWishknowledge = Convert.ToInt32(_session.CreateSQLQuery(query).UniqueResult());
        return countSetsInOtherWishknowledge;
    }

}