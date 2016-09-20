﻿using System;
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
        result.User = user;

        /*Calculate Reputation for Questions and Sets created */

        var createdQuestions = _session.QueryOver<Question>()
            .Where(q => q.Creator.Id == user.Id)
            .And(q => q.Visibility == QuestionVisibility.All)
            .List<Question>();
        result.ForQuestionsCreated = createdQuestions.Count * PointsPerQuestionCreated;

        var createdSets = _session.QueryOver<Set>()
            .Where(s => s.Creator.Id == user.Id)
            .RowCount();
        result.ForSetsCreated = createdSets*PointsPerSetCreated;

        /*Calculate Reputation for Questions and Sets in other user's wish knowledge */

        var countQuestionsInOtherWishknowledge = _session.QueryOver<QuestionValuation>()
            .Where(qv => qv.User != user)
            .And(qv => qv.RelevancePersonal != -1)
            .JoinQueryOver<Question>(qv => qv.Question)
            .Where(q => q.Creator == user)
            .RowCount();
        result.ForQuestionsInOtherWishknowledge = countQuestionsInOtherWishknowledge * PointsPerQuestionInOtherWishknowledge;


        //The following doesn't work, unfortunately (reason: Set is not a property of setValuation)
        //var countSetsInOtherWishknowledge = _session.QueryOver<SetValuation>()
        //    .Where(sv => sv.UserId != user.Id)
        //    .And(qv => qv.RelevancePersonal != -1)
        //    .Left.JoinAlias()
        //    .JoinQueryOver<Set>(sv => sv.Set)
        //    .Where(s => s.Creator == user)
        //    .RowCount();

        //this is the alternative by writing sql-code (it is working)
        var query =
            String.Format(
                @"SELECT count(*)
                FROM setvaluation sv
                LEFT JOIN questionset s
                ON sv.SetId = s.Id
                WHERE s.Creator_id = {0}
                AND sv.UserId <> {0}
                AND sv.RelevancePersonal <> -1",
                user.Id);
        var countSetsInOtherWishknowledge = Convert.ToInt32(_session.CreateSQLQuery(query).UniqueResult());

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

        /* Calculate Reputation for ... */

        result.ForPublicWishknowledge = user.ShowWishKnowledge ? PointsForPublicWishknowledge : 0;
        result.ForUsersFollowingMe = _session.R<TotalFollowers>().Run(user.Id) * PointsPerUserFollowingMe;

        return result;
    }
}