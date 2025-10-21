using System.Collections.Concurrent;
using System.Diagnostics;

public class AnswerQuestionDetailsController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    TotalsPerUserLoader totalsPerUserLoader,
    IHttpContextAccessor _httpContextAccessor,
    ExtendedUserCache _extendedUserCache,
    QuestionReadingRepo _questionReadingRepo,
    QuestionViewRepository _questionViewRepository) : ApiBaseController
{
    [HttpGet]
    public AnswerQuestionDetailsResult? Get([FromRoute] int id)
    {
        var question = EntityCache.GetQuestionById(id);

        if (question.Id == 0 || !_permissionCheck.CanView(question))
            return null;
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        Log.Information("AnswerQuestionDetailsTimer a - {0}", stopWatch.Elapsed.TotalMilliseconds);
        var dateNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var answerQuestionModel = new AnswerQuestionModel(question,
            _sessionUser,
            totalsPerUserLoader,
            _extendedUserCache);

        var correctnessProbability = answerQuestionModel.HistoryAndProbability.CorrectnessProbability;
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;
        var sessionUser = _extendedUserCache.GetItem(_sessionUser.UserId);
        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? sessionUser.QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();
        Log.Information("AnswerQuestionDetailsTimer b - {0}", stopWatch.Elapsed.TotalMilliseconds);
        var hasUserValuation = userQuestionValuation.ContainsKey(question.Id) && _sessionUser.IsLoggedIn;
        stopWatch.Stop();

        var result = new AnswerQuestionDetailsResult(
            QuestionId: question.Id,
            KnowledgeStatus: hasUserValuation
                ? userQuestionValuation[question.Id].KnowledgeStatus
                : KnowledgeStatus.NotLearned,
            PersonalProbability: correctnessProbability.CPPersonal,
            PersonalColor: correctnessProbability.CPPColor,
            AvgProbability: correctnessProbability.CPAll,
            PersonalAnswerCount: history.TimesAnsweredUser,
            PersonalAnsweredCorrectly: history.TimesAnsweredUserTrue,
            PersonalAnsweredWrongly: history.TimesAnsweredUserWrong,
            OverallAnswerCount: history.TimesAnsweredTotal,
            OverallAnsweredCorrectly: history.TimesAnsweredCorrect,
            OverallAnsweredWrongly: history.TimesAnsweredWrongTotal,
            IsInWishKnowledge: answerQuestionModel.HistoryAndProbability.QuestionValuation
                .IsInWishKnowledge,
            Pages: question.PagesVisibleToCurrentUser(_permissionCheck).Select(t =>
                new AnswerQuestionDetailsPageItem(
                    Id: t.Id,
                    Name: t.Name,
                    QuestionCount: t.GetCountQuestionsAggregated(_sessionUser.UserId),
                    ImageUrl: new PageImageSettings(t.Id, _httpContextAccessor)
                        .GetUrl_128px(asSquare: true).Url,
                    MiniImageUrl: new ImageFrontendData(
                            _imageMetaDataReadingRepo.GetBy(t.Id, ImageType.Page),
                            _httpContextAccessor,
                            _questionReadingRepo)
                        .GetImageUrl(30, true, false, ImageType.Page).Url,
                    Visibility: (int)t.Visibility,
                    IsSpoiler: IsSpoilerPage.Yes(t.Name, question)
                )).Distinct().ToArray(),
            Visibility: question.Visibility,
            DateNow: dateNow,
            EndTimer: DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Creator: new MacroCreator(
                Id: question.CreatorId,
                Name: question.Creator.Name
            ),
            CreationDate: question.DateCreated,
            TotalViewCount: _questionViewRepository.GetViewCount(question.Id),
            WishKnowledgeCount: question.TotalRelevancePersonalEntries,
            LicenseId: question.License.Id
        );
        return result;
    }
}