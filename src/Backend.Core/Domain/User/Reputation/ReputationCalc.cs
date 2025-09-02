using ISession = NHibernate.ISession;

public class ReputationCalc(
    ISession session,
    UserReadingRepo userReadingRepo) : IRegisterAsInstancePerLifetime
{
    public const int PointsPerQuestionCreated = 1; //excluding private questions
    public const int PointsPerQuestionInOtherWishknowledge = 5;
    public const int PointsForPublicWishknowledge = 30;

    public ReputationCalcResult Run(UserCacheItem user)
    {
        var result = new ReputationCalcResult();
        result.User = new UserTinyModel(user);

        /*Calculate Reputation for Questions and Sets created */

        var createdQuestions = session.QueryOver<Question>()
            .Where(q => q.Creator != null && q.Creator.Id == result.User.Id)
            .And(q => q.Visibility == QuestionVisibility.Public)
            .List<Question>();
        result.ForQuestionsCreated = createdQuestions.Count * PointsPerQuestionCreated;

        /*Calculate Reputation for Questions, Sets, Pages in other user's wish knowledge */

        var countQuestionsInOtherWishknowledge = userReadingRepo.GetByIds(user.Id);
        result.ForQuestionsInOtherWishknowledge =
            countQuestionsInOtherWishknowledge[0].TotalInOthersWishknowledge *
            PointsPerQuestionInOtherWishknowledge;

        /* Calculate Reputation for other things */

        result.ForPublicWishknowledge = result.User.ShowWishKnowledge ? PointsForPublicWishknowledge : 0;
        // result.ForUsersFollowingMe = _totalFollowers.Run(result.User.Id) * PointsPerUserFollowingMe;

        return result;
    }

    public ReputationCalcResult RunWithQuestionCacheItems(UserCacheItem user)
    {
        var result = new ReputationCalcResult();
        result.User = new UserTinyModel(user);

        /*Calculate Reputation for Questions and Sets created */

        var createdQuestions = EntityCache.GetAllQuestions()
            .Where(q => q.Creator != null &&
                        q.Creator.Id == result.User.Id &&
                        q.Visibility == QuestionVisibility.Public).ToList();
        result.ForQuestionsCreated = createdQuestions.Count * PointsPerQuestionCreated;

        /*Calculate Reputation for Questions, Sets, Pages in other user's wish knowledge */

        result.ForQuestionsInOtherWishknowledge =
            user.TotalInOthersWishknowledge * PointsPerQuestionInOtherWishknowledge;

        /* Calculate Reputation for other things */

        result.ForPublicWishknowledge =
            result.User.ShowWishKnowledge ? PointsForPublicWishknowledge : 0;

        return result;
    }
}