using ISession = NHibernate.ISession;

public class ReputationCalc(
    ISession session,
    UserReadingRepo userReadingRepo) : IRegisterAsInstancePerLifetime
{
    public const int PointsPerQuestionCreated = 1; //excluding private questions
    public const int PointsPerQuestionInOtherWishKnowledge = 5;
    public const int PointsForPublicWishKnowledge = 30;

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

        var countQuestionsInOtherWishKnowledge = userReadingRepo.GetByIds(user.Id);
        result.ForQuestionsInOtherWishKnowledge =
            countQuestionsInOtherWishKnowledge[0].TotalInOthersWishKnowledge *
            PointsPerQuestionInOtherWishKnowledge;

        /* Calculate Reputation for other things */

        result.ForPublicWishKnowledge = result.User.ShowWishKnowledge ? PointsForPublicWishKnowledge : 0;

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

        result.ForQuestionsInOtherWishKnowledge =
            user.TotalInOthersWishKnowledge * PointsPerQuestionInOtherWishKnowledge;

        /* Calculate Reputation for other things */

        result.ForPublicWishKnowledge =
            result.User.ShowWishKnowledge ? PointsForPublicWishKnowledge : 0;

        return result;
    }
}