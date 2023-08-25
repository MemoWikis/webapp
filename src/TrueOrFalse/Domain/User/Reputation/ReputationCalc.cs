using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ISession = NHibernate.ISession;

public class ReputationCalc : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;
    private readonly TotalFollowers _totalFollowers;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public const int PointsPerQuestionCreated = 1; //excluding private questions
    public const int PointsPerQuestionInOtherWishknowledge = 5;
    public const int PointsPerUserFollowingMe = 20;
    public const int PointsForPublicWishknowledge = 30;

    public ReputationCalc(ISession session,
        TotalFollowers totalFollowers,
        UserReadingRepo userReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _session = session;
        _totalFollowers = totalFollowers;
        _userReadingRepo = userReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public ReputationCalcResult Run(UserCacheItem user)
    {
        var result = new ReputationCalcResult();
        result.User = new UserTinyModel(user);

        /*Calculate Reputation for Questions and Sets created */

        var createdQuestions = _session.QueryOver<Question>()
            .Where(q => q.Creator != null && q.Creator.Id == result.User.Id)
            .And(q => q.Visibility == QuestionVisibility.All)
            .List<Question>();
        result.ForQuestionsCreated = createdQuestions.Count * PointsPerQuestionCreated;

        /*Calculate Reputation for Questions, Sets, Categories in other user's wish knowledge */

        var countQuestionsInOtherWishknowledge = _userReadingRepo.GetByIds(user.Id);
        result.ForQuestionsInOtherWishknowledge = countQuestionsInOtherWishknowledge[0].TotalInOthersWishknowledge * PointsPerQuestionInOtherWishknowledge;

        /* Calculate Reputation for other things */

        result.ForPublicWishknowledge = result.User.ShowWishKnowledge ? PointsForPublicWishknowledge : 0;
        result.ForUsersFollowingMe = _totalFollowers.Run(result.User.Id) * PointsPerUserFollowingMe;

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
                        q.Visibility == QuestionVisibility.All).ToList();
        result.ForQuestionsCreated = createdQuestions.Count * PointsPerQuestionCreated;

        /*Calculate Reputation for Questions, Sets, Categories in other user's wish knowledge */

        result.ForQuestionsInOtherWishknowledge = user.TotalInOthersWishknowledge * PointsPerQuestionInOtherWishknowledge;

        /* Calculate Reputation for other things */

        result.ForPublicWishknowledge = result.User.ShowWishKnowledge ? PointsForPublicWishknowledge : 0;
        result.ForUsersFollowingMe = _totalFollowers.Run(result.User.Id) * PointsPerUserFollowingMe;

        return result;
    }
}